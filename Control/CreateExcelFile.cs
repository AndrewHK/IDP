using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using IDPParser.Model;

namespace IDPParser.Control
{
    public static class CreateExcelFile
    {
        public static bool CreateRumorsCompleteExcelDocument(List<TMRumor> rumorsList,
            List<TMRumorSource> rumorsSourcesList, string xlsxFilePath)
        {
            var ds = new DataSet();
            ds.Tables.Add(CustomListToDataTable(rumorsList));
            ds.Tables.Add(ListToDataTable(rumorsSourcesList));

            return CreateExcelDocument(ds, xlsxFilePath);
        }


        public static bool CreateRumorsExcelDocument(List<TMRumor> list, string xlsxFilePath)
        {
            var ds = new DataSet();
            ds.Tables.Add(CustomListToDataTable(list));

            return CreateExcelDocument(ds, xlsxFilePath);
        }

        public static bool CreateExcelDocument<T>(List<T> list, string xlsxFilePath)
        {
            var ds = new DataSet();
            ds.Tables.Add(ListToDataTable(list));

            return CreateExcelDocument(ds, xlsxFilePath);
        }

        /// <summary>
        ///     Create an Excel file, and write it to a file.
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="excelFilename">Name of file to be written.</param>
        /// <returns>True if successful, false if something went wrong.</returns>
        public static bool CreateExcelDocument(DataSet ds, string excelFilename)
        {
            try
            {
                using (
                    SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilename,
                        SpreadsheetDocumentType.Workbook))
                {
                    WriteExcelFile(ds, document);
                }
                Trace.WriteLine("Successfully created: " + excelFilename);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

        private static void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet)
        {
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            var workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            var stylesheet = new Stylesheet();
            workbookStylesPart.Stylesheet = stylesheet;

            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                string workSheetId = "rId" + worksheetNumber;
                string worksheetName = dt.TableName;

                var newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet();

                // create sheet data
                newWorksheetPart.Worksheet.AppendChild(new SheetData());

                // save worksheet
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                newWorksheetPart.Worksheet.Save();
                // create the worksheet to workbook relation
                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());

                spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = worksheetNumber,
                    Name = dt.TableName
                });

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();
        }


        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            var isNumericColumn = new bool[numberOfColumns];

            var excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;

            var headerRow = new Row {RowIndex = rowIndex}; // add a row at the top of spreadsheet
            sheetData.Append(headerRow);

            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];
                AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, headerRow);
                isNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") ||
                                          (col.DataType.FullName == "System.Int32");
            }

            //
            //  Now, step through each row of data in our DataTable...
            //
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row {RowIndex = rowIndex}; // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    string cellValue = dr.ItemArray[colInx].ToString();

                    // Create cell with data
                    if (isNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        double cellNumericValue = 0;
                        if (!double.TryParse(cellValue, out cellNumericValue)) continue;
                        cellValue = cellNumericValue.ToString();
                        AppendNumericCell(excelColumnNames[colInx] + rowIndex, cellValue, newExcelRow);
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex, cellValue, newExcelRow);
                    }
                }
            }
        }

        private static void AppendTextCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            var cell = new Cell
            {
                CellReference = cellReference,
                DataType = CellValues.String
            };
            var cellValue = new CellValue {Text = cellStringValue};
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static void AppendNumericCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            var cell = new Cell {CellReference = cellReference};
            var cellValue = new CellValue {Text = cellStringValue};
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if (columnIndex < 26)
                return ((char) ('A' + columnIndex)).ToString();

            var firstChar = (char) ('A' + (columnIndex/26) - 1);
            var secondChar = (char) ('A' + (columnIndex%26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }

        #region HELPER_FUNCTIONS

        //  This function is adapated from: http://www.codeguru.com/forum/showthread.php?t=450171

        public static DataTable CustomListToDataTable(List<TMRumor> list)
        {
            var dt = new DataTable();

            foreach (PropertyInfo info in typeof (TMRumor).GetProperties())
            {
                if (info.PropertyType == typeof (TMClub) || info.PropertyType == typeof (TMPlayer))
                {
                    foreach (PropertyInfo innerInfo in info.PropertyType.GetProperties())
                    {
                        dt.Columns.Add(new DataColumn(info.Name + "_" + innerInfo.Name,
                            GetNullableType(innerInfo.PropertyType)));
                    }
                }
                else
                {
                    dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
                }
            }
            foreach (TMRumor t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof (TMRumor).GetProperties())
                {
                    if (info.PropertyType == typeof (TMPlayer))
                    {
                        foreach (PropertyInfo innerInfo in typeof (TMPlayer).GetProperties())
                        {
                            if (!IsNullableType(innerInfo.PropertyType))
                                row[info.Name + "_" + innerInfo.Name] = innerInfo.GetValue(t.Player, null);
                            else
                                row[info.Name + "_" + innerInfo.Name] = (innerInfo.GetValue(t.Player, null) ??
                                                                         DBNull.Value);
                        }
                    }
                    else if (info.PropertyType == typeof (TMClub) && info.Name.Equals("CurrentClub"))
                    {
                        foreach (PropertyInfo innerInfo in typeof (TMClub).GetProperties())
                        {
                            if (!IsNullableType(innerInfo.PropertyType))
                                row[info.Name + "_" + innerInfo.Name] = innerInfo.GetValue(t.CurrentClub, null);
                            else
                                row[info.Name + "_" + innerInfo.Name] = (innerInfo.GetValue(t.CurrentClub, null) ??
                                                                         DBNull.Value);
                        }
                    }
                    else if (info.PropertyType == typeof (TMClub) && info.Name.Equals("InterestedClub"))
                    {
                        foreach (PropertyInfo innerInfo in typeof (TMClub).GetProperties())
                        {
                            if (!IsNullableType(innerInfo.PropertyType))
                                row[info.Name + "_" + innerInfo.Name] = innerInfo.GetValue(t.InterestedClub, null);
                            else
                                row[info.Name + "_" + innerInfo.Name] = (innerInfo.GetValue(t.InterestedClub, null) ??
                                                                         DBNull.Value);
                        }
                    }
                    else
                    {
                        if (!IsNullableType(info.PropertyType))
                            row[info.Name] = info.GetValue(t, null);
                        else
                            row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            var dt = new DataTable();

            foreach (PropertyInfo info in typeof (T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof (T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }

        private static bool IsNullableType(Type type)
        {
            return (type == typeof (string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition() == typeof (Nullable<>)));
        }

        public static bool CreateExcelDocument(DataTable dt, string xlsxFilePath)
        {
            var ds = new DataSet();
            ds.Tables.Add(dt);
            bool result = CreateExcelDocument(ds, xlsxFilePath);
            ds.Tables.Remove(dt);
            return result;
        }

        #endregion
    }
}