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
        
        private async void CrawlBtn_Click(object sender, EventArgs e)
        {

            const string clublistXml = "clubList.xml";

            const string backupRumorsFile = "rumorlist.xlsx";
            const string backupRumorsSourcesFile = "rumorlistsources.xlsx";

            var url = new Uri(rumorMillTB.Text);

            /*
            Utils.RetrieveClubs(filename);
            */
            var limitedList = new List<string> { "972770" };

            //http://www.transfermarkt.de/geruchtekuche/detail/forum/154/
            //http://www.transfermarkt.de/rumour-mill/detail/forum/500/

            const int jumpCount = 50;

            _tmParser.DetermineForumPageCount(url);
            for (var i = 51; i < 401; i += jumpCount)
            {
                string outFilename = string.Format("RumorMill_German({0}-{1}).xlsx", i, i + jumpCount-1);

                try
                {
                    _tmParser.ParseForum(i, i + jumpCount-1);
                    await _tmParser.NavigateToRumorPages();

                    //MessageBox.Show("Navigation done!");
                    //navGB.BackColor = System.Drawing.Color.MediumSeaGreen;

                    //Utils.SerializeObject(_tmParser.GetRumorsList(), backupRumorsFile);
                    //Utils.SerializeObject(_tmParser.GetRumorsSourcesList(), backupRumorsSourcesFile);

                    _tmParser.UpdateRumorsSources();
                    //MessageBox.Show("Rumor Sources updated!");
                    //rumorSrcGB.BackColor = System.Drawing.Color.MediumSeaGreen;


                    _tmParser.UpdateInterestedClubs(clublistXml);
                    //MessageBox.Show("Interested Clubs retrieved!");
                    //interClubGB.BackColor = System.Drawing.Color.MediumSeaGreen;

                    _tmParser.DetermineRumorType();
                    //MessageBox.Show("Rumor Types determined!");
                    //rumorTypeGB.BackColor = System.Drawing.Color.MediumSeaGreen;

                    CreateExcelFile.CreateRumorsCompleteExcelDocument(_tmParser.GetRumorsList(),
                        _tmParser.GetRumorsSourcesList(), outFilename);
                    //MessageBox.Show("Excel sheet created!");
                    //excelSheetGB.BackColor = System.Drawing.Color.MediumSeaGreen;
                }
                catch
                {
                    
                }
                _tmParser.Refresh();
            }
           
            
        }
    }
}