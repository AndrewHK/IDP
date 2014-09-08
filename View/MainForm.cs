/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 2:58 PM
 * 
 */

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            _tmParser = new TMHTMLParser(logTB);
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        private void CrawlBtnClick(object sender, EventArgs e)
        {
            //string pu = "http://www.transfermarkt.de/shinji-kagawa/profil/spieler/81785";
            //_tmParser.ParsePlayer(pu);
            /*
            const string filename = "clubList.xml";
            Utils.RetrieveClubs(filename);
            */
             
            _tmParser.ParseForum();
            var task = _tmParser.NavigateToRumorPages();
            task.ContinueWith(t =>
            {
                MessageBox.Show("Navigation done!");
                _tmParser.UpdateRumorsSources();
                MessageBox.Show("Rumor Sources updated!");

                _tmParser.UpdateInterestedClubs();
                MessageBox.Show("Interested Clubs retrieved!");

                CreateExcelFile.CreateRumorsCompleteExcelDocument(_tmParser.GetRumorsList(),
                    _tmParser.GetRumorsSourcesList(), "Sample_Complete.xlsx");
                MessageBox.Show("Excel sheet created!");

            }, TaskScheduler.FromCurrentSynchronizationContext());
            
        }
    }
}