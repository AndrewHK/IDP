/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:26 PM
 * 
*/

using System;
using System.Globalization;
using System.Linq;

namespace IDPParser.Model
{
    /// <summary>
    ///     Description of TMRumorSource.
    /// </summary>
    public class TMRumorSource
    {

        public TMRumorSource(string rumorId, string headline, string date, string name, string url, string text)
        {
            RumorId = rumorId;
            Headline = headline;
            Date = date;
            Name = name;
            Url = url;
            Text = text;
        }

        public string RumorId { get; set; }
        public string Player
        {
            get { return string.Format(" ID={0}, Name={1}", _player.Id, _player.Name); }
        }
        public string CurrentClub
        {
            get {return string.Format(" ID={0}, Name={1}", _currentClub.Id, _currentClub.Name);}
        }
        public string Date { get; set; }
        public string Headline { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        

        private TMClub _currentClub;
        private TMPlayer _player;
        private const string DateFormat = "dd.MM.yyyy - HH:mm";

        public void SetPlayer(TMPlayer player)
        {
            _player = player;
        }

        public string GetCurrentClubId()
        {
            return _currentClub.Id;
        }

        public void DetermineCurrentClub()
        {
            //var keysCollection = Player.TransferDictionary.Keys;
            
            var sourceDate = DateTime.ParseExact(Date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
            var playerTransfers = _player.GetTransferDictionary();
            /* Loop on the SORTED dictionary till it finds a date that is after the given rumor source date,
             * Get the club related to the date BEFORE the one it found
             * If nothing before, current club is null
             * If nothing after, current club is last club
             */
            var prevKey = DateTime.MinValue;
            foreach (var kvp in playerTransfers)
            {
                if (kvp.Key.CompareTo(sourceDate) > 0)
                {
                    _currentClub = playerTransfers[prevKey];
                    return;
                }
                prevKey = kvp.Key;
            }
            _currentClub = playerTransfers[prevKey];
        }
    }
}