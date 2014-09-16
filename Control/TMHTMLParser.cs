﻿/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:12 PM
 * 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
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

        private readonly TextBox _logger;
        public void AppendLog(string text)
        {
            _logger.AppendText(text);
            _logger.AppendText("\r\n");
        }
        public TMHTMLParser(TextBox logBox)
        {
            DomainUrl = "http://www.transfermarkt.de";
            RumorMillUrl = "http://www.transfermarkt.de/rumour-mill/detail/forum/500/";
            ForumPages = 10;
            _rumorList = new List<TMRumor>();
            _rumorSourcesList = new List<TMRumorSource>();

            _logger = logBox;
            SetWebBrowsers();
        }

        public string DomainUrl { get; set; }
        public string RumorMillUrl { get; set; }
        public int ForumPages { get; set; }

        private void SetWebBrowsers()
        {
            _mainWb = new WebBrowser { ScriptErrorsSuppressed = true };
            _playerWb = new WebBrowser { ScriptErrorsSuppressed = true };
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
                AppendLog(string.Format(r.ToString()));
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


        public void ParseForum(List<string> limitedList = null)
        {
            for (var i = 1; i <= 10; i++)
            {
                var link = string.Format("{0}page/{1}", RumorMillUrl, i);

                var htmlData = Utils.GetHtmlFromUrl(link);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlData);

                if (htmlDoc.DocumentNode == null) return;
                foreach (
                    var linkNode in
                        htmlDoc.DocumentNode.SelectNodes("//a[" +
                                                         Utils.GenerateClassSelectorString(
                                                             "board_titel spielprofil_tooltip") +
                                                         "]"))
                {
                    var att = linkNode.Attributes["href"];

                    var noOfPages = (linkNode.ParentNode.NextSibling.NextSibling.ChildNodes.Count > 1)
                        ? Int16.Parse(linkNode.ParentNode.NextSibling.NextSibling.LastChild.PreviousSibling.InnerText)
                        : 1;
                    var url = DomainUrl + att.Value;
                    var id = url.Substring(url.LastIndexOf('/') + 1);
                    var title = linkNode.InnerText.Trim();

                    if (limitedList != null && !limitedList.Contains(id))
                    {
                        continue;
                    }
                    _rumorList.Add(new TMRumor(id, title, url, noOfPages));
                }
            }
            AppendLog(string.Format("{0} forum entries were found", _rumorList.Count));
        }

        public async Task NavigateToRumorPages()
        {
            TaskCompletionSource<bool> isRumorPageCompleted = null;

            _mainWb.DocumentCompleted += (s, e) =>
            {
                if (_mainWb.ReadyState != WebBrowserReadyState.Complete)
                    return;
                if (isRumorPageCompleted.Task.IsCompleted)
                    return;
                AppendLog(string.Format("One Page is loaded"));
                isRumorPageCompleted.SetResult(true);
            };

            var counter = 1;
            foreach (var rumor in _rumorList)
            {
                _currRumorId = rumor.Id;
                int i;
                var isRumorOk = false;
                const int timeout = 10000;
                for (i = 1; i <= rumor.NoOfPages; i++)
                {
                    isRumorPageCompleted = new TaskCompletionSource<bool>();
                    AppendLog(string.Format("One Page is pending navigation, ID: {0}", rumor.Id));
                    var rumorUrLpaged = rumor.Url + "/page/" + i;
                    //navgiate to rumorURLpaged and wait for notification to parseRumor
                    _mainWb.Navigate(rumorUrLpaged);


                    var completionTask = isRumorPageCompleted.Task;
                    if (await Task.WhenAny(completionTask, Task.Delay(timeout)) == completionTask)
                    {
                        isRumorOk = ParseRumor();
                        if (isRumorOk)
                        {
                            AppendLog(string.Format("One Page is parsed, ID: {0}  URL: {1}", rumor.Id, _mainWb.Url.AbsolutePath));
                        }
                    }
                    else
                    {
                        //Navigate again to the page
                        i--;
                        AppendLog(string.Format("One Page had a timeout, ID: {0}, URL: {1}", rumor.Id, _mainWb.Url.AbsolutePath));
                    }
                }
                if (isRumorOk)
                {
                    await NavigateToPlayerPage(rumor.Player);
                    AppendLog(string.Format("Rumor: {0}, {1} page(s) ---- {2}", rumor.Id, i - 1, counter++));
                }
            }
        }

        public async Task NavigateToPlayerPage(TMPlayer player)
        {
            TaskCompletionSource<bool> isPlayerPageCompleted = null;
            const int timeout = 10000;

            _playerWb.DocumentCompleted += (s, e) =>
            {
                if (_playerWb.ReadyState != WebBrowserReadyState.Complete)
                    return;
                if (isPlayerPageCompleted.Task.IsCompleted)
                    return;
                AppendLog(string.Format("Player Page is loaded"));
                isPlayerPageCompleted.SetResult(true);
            };
            while (true)
            {
                isPlayerPageCompleted = new TaskCompletionSource<bool>();
                //navgiate to player url and wait for notification to parsePlayer
                var playerUrl = DomainUrl + player.Url;
                _playerWb.Navigate(playerUrl);

                var completionTask = isPlayerPageCompleted.Task;
                if (await Task.WhenAny(completionTask, Task.Delay(timeout)) == completionTask)
                {
                    var isPlayerOk = ParsePlayer();
                    if (isPlayerOk)
                    {
                        AppendLog(string.Format("Player is parsed, ID: {0}  URL: {1}", _rumorList[GetRumorIndex()].Player.Id,
                            _playerWb.Url.AbsolutePath));
                    }
                    break;
                }
                //Navigate again to the page
                AppendLog(string.Format("Player Page had a timeout, URL: {0}", _playerWb.Url.AbsolutePath));
            }
        }

        private bool ParseRumor()
        {
            var htmlData = _mainWb.DocumentText;
            var ri = GetRumorIndex();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlData);

            //div[contains(concat(' ',normalize-space(@class),' '),' forum-post-data ')]/div[]
            var xpath = "//div[" + Utils.GenerateClassSelectorString("forum-post-data") +
                                                         "]/div[" + Utils.GenerateClassSelectorString("box-header-forum") +
                                                         "]";
            if (htmlDoc.DocumentNode == null) return false;
            try
            {
                var isFixedSourceNotParsed = true;
                //Get Sources
                foreach (
                    var sourceDiv in
                        htmlDoc.DocumentNode.SelectNodes(xpath))
                {
                    try
                    {
                        if (_rumorList[ri].IsFixedSourceExist && isFixedSourceNotParsed)
                        {
                            isFixedSourceNotParsed = false;
                            continue;
                        }

                        var url = sourceDiv.SelectSingleNode("a").Attributes["href"].Value;
                        var name = sourceDiv.InnerText.Substring("QUELLE: ".Length).Trim();
                        var text = sourceDiv.NextSibling.InnerText;

                        var tempNodes =
                            sourceDiv.SelectNodes("preceding::div[" + Utils.GenerateClassSelectorString("forum-datum") +
                                                  "]");

                        var dateNode = tempNodes[tempNodes.Count - 1];
                        var dateWithUhr = dateNode.InnerText.Trim();

                        var date = dateWithUhr.Substring(0, dateWithUhr.Length - 3).Trim();

                        var rumorSource = new TMRumorSource(_rumorList[ri].Id, date, name, url, text);
                        _rumorList[ri].AddSource(rumorSource);
                        _rumorSourcesList.Add(rumorSource);
                    }
                    catch (NullReferenceException e)
                    {
                        AppendLog(string.Format("Skipping an invalid Rumor Source : {0}, ", e.Message));
                    }
                }

                if (_rumorList[ri].IsParsed) return true;
                //Get Player
                var playerTitleNode =
                    htmlDoc.DocumentNode.SelectSingleNode("//div[" + Utils.GenerateClassSelectorString("spielername-profil") +
                                                          "]");
                var playerName = playerTitleNode.InnerText;
                var playerUrl = playerTitleNode.SelectSingleNode("a").Attributes["href"].Value;
                var playerId = playerUrl.Substring(playerUrl.LastIndexOf('/') + 1);
                var playerDetailsNode =
                    htmlDoc.DocumentNode.SelectSingleNode("//table[" + Utils.GenerateClassSelectorString("profilheader") + "]");
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
                //_rumorList[ri].CurrentClub = new TMClub(currentClubId, currentClubName);
                _rumorList[ri].IsParsed = true;

                // Check if there is a fixed source
                var fixedSourceNode =
                    htmlDoc.DocumentNode.SelectSingleNode("//div[" + Utils.GenerateClassSelectorString("box") + "]");
                _rumorList[ri].IsFixedSourceExist = fixedSourceNode != null;
                return true;
            }
            catch (Exception e)
            {
                AppendLog(string.Format("Rumor has incomplete details : {0}", e.Message));
                return false;
            }
        }

        public bool ParsePlayer()
        {
            var htmlData = _playerWb.DocumentText;
            var ri = GetRumorIndex();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlData);

            if (htmlDoc.DocumentNode == null) return false;

            //div[contains(concat(' ',normalize-space(@class),' '),' eight columns ')]/div/div[contains(concat(' ',normalize-space(@class),' '),' responsive-table ')]/table/tbody/tr
            var xpath = "//div[" + Utils.GenerateClassSelectorString("eight columns") + "]/div/div[" +
                        Utils.GenerateClassSelectorString("responsive-table") + "]/table/tbody/tr";
            Exception errorDetected = null;
            foreach (
                var transferNodeRow in
                    htmlDoc.DocumentNode.SelectNodes(xpath))
            {
                try
                {
                    var leihe = transferNodeRow.ChildNodes[19].InnerText;
                    if (leihe.Trim().Length != 0)
                        continue;

                    var date = transferNodeRow.ChildNodes[3].LastChild.InnerText;

                    var currentClubName = transferNodeRow.ChildNodes[15].InnerText.Trim();
                    var currentClubUrl = transferNodeRow.ChildNodes[15].ChildNodes[1].Attributes["href"].Value;

                    var begin = currentClubUrl.LastIndexOf("verein/", StringComparison.Ordinal) + 7;
                    var end = currentClubUrl.LastIndexOf("/saison", StringComparison.Ordinal);
                    var currentClubId = currentClubUrl.Substring(begin, end - begin);

                    var currentClub = new TMClub(currentClubId, currentClubName);

                    _rumorList[ri].Player.AddTransfer(date, currentClub);
                    errorDetected = null;
                }
                catch (Exception e)
                {
                    errorDetected = e;
                }
            }

            if (errorDetected == null) return true;
            AppendLog(string.Format("Player has incomplete details : {0}", errorDetected.Message));
            return false;
        }

        public void UpdateRumorsSources()
        {
            var splitRumorList = new List<TMRumor>();
            foreach (var rumorSource in _rumorSourcesList)
            {
                var ri = GetRumorIndex(rumorSource.RumorId);
                var player = _rumorList[ri].Player;

                rumorSource.SetPlayer(player);
                rumorSource.DetermineCurrentClub();
            }
            foreach (var rumor in _rumorList)
            {
                var rumorSources = rumor.GetSources();
                if(rumorSources == null || rumorSources.Count == 0) continue;

                var rumorCurrentClubId = rumorSources[0].CurrentClubId;
                var detectedCurrentClubsIdList = new List<string>();

                var postfix = (char)64;
                foreach (
                    var rumorSource in
                        rumorSources.Where(
                            rumorSource =>
                                (!string.IsNullOrEmpty(rumorSource.CurrentClubId)) &&
                                (!rumorCurrentClubId.Equals(rumorSource.CurrentClubId))))
                {
                    if (detectedCurrentClubsIdList.IndexOf(rumorSource.CurrentClubId) < 0)
                    {
                        detectedCurrentClubsIdList.Add(rumorSource.CurrentClubId);
                        postfix++;

                        var newSplitRumor = new TMRumor(rumor) { Id = rumorSource.RumorId + postfix };
                        //newSplitRumor.CurrentClub.Id = rumorSource.CurrentClubId;
                        //newSplitRumor.CurrentClub.Name = rumorSource.CurrentClubName;
                        splitRumorList.Add(newSplitRumor);
                    }
                    rumorSource.SplitRumor = rumorSource.RumorId + postfix;
                }
            }
            _rumorList.AddRange(splitRumorList);
        }

        public void UpdateInterestedClubs(string filename)
        {
            var clubList = Utils.DeSerializeObject<List<TMClub>>(filename);

            AppendLog("Getting Clubs from stored file ..");
            AppendLog(string.Format("Found {0} clubs ! Checking the interested club for each rumor source", clubList.Count));

            foreach (var rumor in _rumorList)
            {
                var rumorTitle = rumor.Title;
                var interestedClub = clubList.FirstOrDefault(c => c.ContainsName(rumorTitle));

                var currRumorSources = rumor.GetSources();
                if (currRumorSources == null || currRumorSources.Count == 0) continue;

                foreach (var rumorSource in currRumorSources)
                {
                    rumorSource.SetInterestedClub(interestedClub);
                }
                AppendLog(string.Format("Rumor ({0}) : Club ({1})", rumor.Title, interestedClub == null ? string.Empty : interestedClub.Name));
            }
        }

        public void DetermineRumorType()
        {
            AppendLog("Getting Types for each Rumor ..");

            foreach (var rumor in _rumorList)
            {
                var currRumorSources = rumor.GetSources();
                if (currRumorSources == null || currRumorSources.Count == 0) continue;

                rumor.Type = currRumorSources.First().IsRumorSuccessful() ? TMRumorType.Sucessful : TMRumorType.Rumor;
                AppendLog(string.Format("Rumor ({0}) : Type ({1})", rumor.Title, rumor.Type));
            }
        }
    }
}