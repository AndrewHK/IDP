/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 2:58 PM
 * 
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Office2010.Excel;
using IDPParser.Control;

namespace IDPParser.View
{
    /// <summary>
    ///     Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly TMHTMLParser _tmParser;

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            _tmParser = new TMHTMLParser(logTB, progressBar, countLB, maxLB);
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
        
        private void CrawlBtn_Click(object sender, EventArgs e)
        {
            //string pu = "http://www.transfermarkt.de/shinji-kagawa/profil/spieler/81785";
            //_tmParser.ParsePlayer(pu);

            const string filename = "../../Data/clubList.xml";
            /*
            Utils.RetrieveClubs(filename);
            */
            //var limitedList = new List<string> { "1007217", "1007227", "1007210", "1007215" };
            _tmParser.ParseForum();
            var task = _tmParser.NavigateToRumorPages();
            task.ContinueWith(t =>
            {
                MessageBox.Show("Navigation done!");
                navGB.BackColor = System.Drawing.Color.MediumSeaGreen;

                _tmParser.UpdateRumorsSources();
                MessageBox.Show("Rumor Sources updated!");
                rumorSrcGB.BackColor = System.Drawing.Color.MediumSeaGreen;


                _tmParser.UpdateInterestedClubs(filename);
                MessageBox.Show("Interested Clubs retrieved!");
                interClubGB.BackColor = System.Drawing.Color.MediumSeaGreen;

                _tmParser.DetermineRumorType();
                MessageBox.Show("Rumor Types determined!");
                rumorTypeGB.BackColor = System.Drawing.Color.MediumSeaGreen;

                CreateExcelFile.CreateRumorsCompleteExcelDocument(_tmParser.GetRumorsList(),
                    _tmParser.GetRumorsSourcesList(), "Sample_Complete.xlsx");
                MessageBox.Show("Excel sheet created!");
                excelSheetGB.BackColor = System.Drawing.Color.MediumSeaGreen;

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}