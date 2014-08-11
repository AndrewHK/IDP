/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:12 PM
 * 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDPParser.Model;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace IDPParser.Control
{
    /// <summary>
    ///     Description of TMHTMLParser.
    /// </summary>
    public class TMHTMLParser
    {
        private readonly List<TMRumor> _rumorList;
        private readonly List<TMRumorSource> _rumorSourcesList;

        private string _currRumorId;

        private WebBrowser _mainWb;
        private WebBrowser _playerWb;

        public TMHTMLParser()
        {
            DomainUrl = "http://www.transfermarkt.de";
            RumorMillUrl = "http://www.transfermarkt.de/rumour-mill/detail/forum/500/";
            _rumorList = new List<TMRumor>();
            _rumorSourcesList = new List<TMRumorSource>();

            SetWebBrowsers();
        }

        public string DomainUrl { get; set; }
        public string RumorMillUrl { get; set; }

        private void SetWebBrowsers()
        {
            _mainWb = new WebBrowser {ScriptErrorsSuppressed = true};
            _playerWb = new WebBrowser { ScriptErrorsSuppressed = true };
        }

        private string GetHtmlFromUrl(string urlAddress)
        {
            var request = (HttpWebRequest) WebRequest.Create(RumorMillUrl);

            // Set some reasonable limits on resources used by this request
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            // Set credentials to use for this request.
            request.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
            var response = (HttpWebResponse) request.GetResponse();

            // Get the stream associated with the response.
            var receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            var readStream = new StreamReader(receiveStream, Encoding.UTF8);

            Console.WriteLine("Response stream received.");
            var source = readStream.ReadToEnd();
            response.Close();
            readStream.Close();

            return source;
        }

        private int GetRumorIndex(string rumorId = null)
        {

            var rumor = rumorId ?? _currRumorId;
            for (var i = 0; i < _rumorList.Count; i++)
            {
                if (_rumorList[i].Id.Equals(rumor))
                    return i;
            }
            return -1;
        }

        public void Testlog()
        {
            foreach (var r in _rumorList)
            {
                Debug.Print(r.ToString());
            }
        }

        public List<TMRumor> GetRumorsList()
        {
            return _rumorList;
        }

        public List<TMRumorSource> GetRumorsSourcesList()
        {
            return _rumorSourcesList;
        }

        private string generateClassSelectorString(string className)
        {
            return "contains(concat(' ',normalize-space(@class),' '),' " + className + " ')";
        }

        public void ParseForum()
        {
            var htmlData = GetHtmlFromUrl(RumorMillUrl);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlData);

            if (htmlDoc.DocumentNode == null) return;
            foreach (
                var linkNode in
                    htmlDoc.DocumentNode.SelectNodes("//a[" +
                                                     generateClassSelectorString("board_titel spielprofil_tooltip") +
                                                     "]"))
            {
                var att = linkNode.Attributes["href"];

                var noOfPages = (linkNode.ParentNode.NextSibling.NextSibling.ChildNodes.Count > 1)
                    ? Int16.Parse(linkNode.ParentNode.NextSibling.NextSibling.LastChild.PreviousSibling.InnerText)
                    : 1;
                var url = DomainUrl + att.Value;
                var id = url.Substring(url.LastIndexOf('/') + 1);
                _rumorList.Add(new TMRumor(id, url, noOfPages));
            }
        }

        public async Task NavigateToRumorPages()
        {
            TaskCompletionSource<bool> isRumorPageCompleted = null;

            _mainWb.DocumentCompleted += (s, e) =>
            {
                Debug.Print("One Page is trying to load");
                if (_mainWb.ReadyState != WebBrowserReadyState.Complete)
                    return;
                if (isRumorPageCompleted.Task.IsCompleted)
                    return;
                Debug.Print("One Page is loaded");
                isRumorPageCompleted.SetResult(true);
            };

            var counter = 1;
            foreach (var rumor in _rumorList)
            {
                _currRumorId = rumor.Id;
                int i;
                const int timeout = 10000;
                for (i = 1; i <= rumor.NoOfPages; i++)
                {
                    isRumorPageCompleted = new TaskCompletionSource<bool>();
                    Debug.Print("One Page is pending navigation, ID: {0}", rumor.Id);
                    var rumorUrLpaged = rumor.Url + "/page/" + i;
                    //navgiate to rumorURLpaged and wait for notification to parseRumor
                    _mainWb.Navigate(rumorUrLpaged);


                    var completionTask = isRumorPageCompleted.Task;
                    if (await Task.WhenAny(completionTask, Task.Delay(timeout)) == completionTask)
                    {
                        ParseRumor();
                        Debug.Print("One Page is parsed, ID: {0}  URL: {1}", rumor.Id, _mainWb.Url.AbsolutePath);
                    }
                    else
                    {
                        //Navigate again to the page
                        i--;
                        Debug.Print("One Page had a timeout, ID: {0}, URL: {1}", rumor.Id, _mainWb.Url.AbsolutePath);
                    }
                }
                await NavigateToPlayerPage(rumor.Player);
                Debug.Print("Rumor: {0}, {1} page(s) ---- {2}", rumor.Id, i - 1, counter++);
            }
        }

        public async Task NavigateToPlayerPage(TMPlayer player)
        {
            TaskCompletionSource<bool> isPlayerPageCompleted = null;
            const int timeout = 10000;

            _playerWb.DocumentCompleted += (s, e) =>
            {
                Debug.Print("Player Page is trying to load");
                if (_playerWb.ReadyState != WebBrowserReadyState.Complete)
                    return;
                if (isPlayerPageCompleted.Task.IsCompleted)
                    return;
                Debug.Print("Player Page is loaded");
                isPlayerPageCompleted.SetResult(true);
            };

           while(true)
           { 
                isPlayerPageCompleted = new TaskCompletionSource<bool>();
                //navgiate to player url and wait for notification to parsePlayer
                _playerWb.Navigate(DomainUrl+ player.Url);
            
                var completionTask = isPlayerPageCompleted.Task;
                if (await Task.WhenAny(completionTask, Task.Delay(timeout)) == completionTask)
                {
                    ParsePlayer();
                    break;
                }
                //Navigate again to the page
                Debug.Print("Player Page had a timeout, URL: {0}", _playerWb.Url.AbsolutePath);
            
            }            
        }

        private void ParseRumor()
        {
            var htmlData = _mainWb.DocumentText;
            var ri = GetRumorIndex();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlData);

            //div[contains(concat(' ',normalize-space(@class),' '),' forum-post-data ')]/div[]

            if (htmlDoc.DocumentNode == null) return;
            //Get Sources
            var isFirstSourceNode = true;
            foreach (
                var sourceDiv in
                    htmlDoc.DocumentNode.SelectNodes("//div[" + generateClassSelectorString("forum-post-data") +
                                                     "]/div[" + generateClassSelectorString("box-header-forum") +
                                                     "]"))
            {
                if (_rumorList[ri].FixedSourceExists && isFirstSourceNode)
                {
                    isFirstSourceNode = false;
                    continue;
                }

                var url = sourceDiv.SelectSingleNode("a").Attributes["href"].Value;
                var name = sourceDiv.InnerText.Substring("QUELLE: ".Length).Trim();
                var text = sourceDiv.NextSibling.InnerText;

                var tempNodes =
                    sourceDiv.SelectNodes("preceding::div[" + generateClassSelectorString("forum-datum") + "]");
                var dateNode = tempNodes[tempNodes.Count - 1];
                var date = dateNode.InnerText.Trim();

                tempNodes = sourceDiv.SelectNodes("preceding::div[" + generateClassSelectorString("fs17") + "]");
                var headlineNode = tempNodes[0];
                var headline = headlineNode.FirstChild.InnerText.Trim();

                var rumorSource = new TMRumorSource(_rumorList[ri].Id, headline, date, name, url, text);
                _rumorList[ri].AddSource(rumorSource);
                _rumorSourcesList.Add(rumorSource);
            }

            if (_rumorList[ri].IsParsed) return;
            //Get Player
            var playerTitleNode =
                htmlDoc.DocumentNode.SelectSingleNode("//div[" + generateClassSelectorString("spielername-profil") +
                                                      "]");
            var playerName = playerTitleNode.InnerText;
            var playerUrl = playerTitleNode.SelectSingleNode("a").Attributes["href"].Value;
            var playerId = playerUrl.Substring(playerUrl.LastIndexOf('/') + 1);
            var playerDetailsNode =
                htmlDoc.DocumentNode.SelectSingleNode("//table[" + generateClassSelectorString("profilheader") + "]");
            var playerDob = playerDetailsNode.ChildNodes[1].LastChild.PreviousSibling.InnerText;
            var playerAge = playerDetailsNode.ChildNodes[3].LastChild.PreviousSibling.InnerText;
            var playerNationality = playerDetailsNode.ChildNodes[5].LastChild.PreviousSibling.InnerText.Trim();
            playerNationality = playerNationality.Substring(playerNationality.LastIndexOf(';') + 1);

            var playerHeight = playerDetailsNode.ChildNodes[7].LastChild.PreviousSibling.InnerText;

            var playerContractValidity = playerDetailsNode.ChildNodes[9].LastChild.PreviousSibling.InnerText;
            var playerPosition = playerDetailsNode.ChildNodes[11].LastChild.PreviousSibling.InnerText;
            var playerStrongerFoot = playerDetailsNode.ChildNodes[13].LastChild.PreviousSibling.InnerText.Trim();
            _rumorList[ri].Player = new TMPlayer(playerId, playerUrl, playerName, playerDob, playerHeight,
                playerPosition, playerNationality, playerStrongerFoot);

            //Get Clubs
            var currentClubName = playerDetailsNode.ChildNodes[15].LastChild.PreviousSibling.InnerText;
            var currentClubUrl =
                playerDetailsNode.ChildNodes[15].LastChild.PreviousSibling.SelectSingleNode("a").Attributes["href"]
                    .Value;
            var currentClubId = currentClubUrl.Substring(currentClubUrl.LastIndexOf('/') + 1);
            _rumorList[ri].CurrentClub = new TMClub(currentClubId, currentClubUrl, currentClubName);
            _rumorList[ri].IsParsed = true;

            // Check if there is a fixed source
            var fixedSourceNode =
                htmlDoc.DocumentNode.SelectSingleNode("//div[" + generateClassSelectorString("box") + "]");
            _rumorList[ri].FixedSourceExists = fixedSourceNode != null;
        }

        public void ParsePlayer()
        {
            var htmlData = _playerWb.DocumentText;
            var ri = GetRumorIndex();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlData);

            if (htmlDoc.DocumentNode == null) return;

            //html/body/div[2]/div[10]/div[1]/div[2]/div[3]/table/tbody/tr[1]/td[2]/a
            
            foreach (
                var transferNodeRow in
                    htmlDoc.DocumentNode.SelectNodes("//div[" +
                                                     generateClassSelectorString("eight columns") +
                                                     "]/div[2]/div[3]/table/tbody/tr"))
            {
                var leihe = transferNodeRow.ChildNodes[19].InnerText;
                if (leihe == null || leihe.Trim().Length == 0)
                    continue;

                var date = transferNodeRow.ChildNodes[3].LastChild.InnerText;

                var currentClubName = transferNodeRow.ChildNodes[15].InnerText.Trim();
                var currentClubUrl = transferNodeRow.ChildNodes[15].ChildNodes[1].Attributes["href"].Value;
                
                var begin = currentClubUrl.LastIndexOf("verein/", StringComparison.Ordinal) + 7;
                var end = currentClubUrl.LastIndexOf("/saison", StringComparison.Ordinal);
                var currentClubId = currentClubUrl.Substring(begin, end-begin);

                var currentClub = new TMClub(currentClubId, currentClubUrl, currentClubName);

                _rumorList[ri].Player.AddTransfer(date, currentClub);
                
            }

        }

        public void UpdateRumorsSources()
        {
            for (var i = 0; i < _rumorSourcesList.Count ; i++)
            {
                var ri = GetRumorIndex(_rumorSourcesList[i].RumorId);
                var player = _rumorList[ri].Player;

                _rumorSourcesList[i].Player = player;
                _rumorSourcesList[i].DetermineCurrentClub();
            }
        }
    }
}