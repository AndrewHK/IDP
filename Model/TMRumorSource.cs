/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:26 PM
 * 
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace IDPParser.Model
{
    /// <summary>
    ///     Description of TMRumorSource.
    /// </summary>
    public class TMRumorSource
    {

        public TMRumorSource(string rumorId, string date, string name, string url, string text)
        {
            RumorId = rumorId;
            _date = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
            
            Name = name;
            Url = url;
            Text = text;
        }

        public string RumorId { get; set; }
        public string SplitRumor { set; get; }
        public string PlayerId
        {
            get { return _player.Id; }
        }
        public string PlayerName
        {
            get { return _player.Name; }
        }
        public string CurrentClubId
        {
            get { return _currentClub != null ? _currentClub.Id : string.Empty; }
        }
        public string CurrentClubName
        {
            get { return _currentClub != null ? _currentClub.Name : string.Empty; }
        }
        public string InterestedClubId
        {
            get { return _interestedClub != null ? _interestedClub.Id : string.Empty; }
        }
        public string InterestedClubName
        {
            get { return _interestedClub != null ? _interestedClub.Name : string.Empty; }
        }

        public string Date
        {
            get
            {
                return _date.ToShortDateString();
            }
        }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }


        private DateTime _date;
        private TMClub _currentClub;
        private TMClub _interestedClub;
        private TMPlayer _player;
        private const string DateFormat = "dd.MM.yyyy - HH:mm";

        public void SetPlayer(TMPlayer player)
        {
            _player = player;
        }

        public bool IsRumorSuccessful()
        {
            var playerTransfers = _player.GetTransferDictionary();

            try
            {
                var currClubKvp = playerTransfers.FirstOrDefault(x => x.Value.Id.Equals(CurrentClubId));
                var interestedClubKvp = playerTransfers.FirstOrDefault(x => x.Value.Id.Equals(InterestedClubId));

                if (_date.Date.CompareTo(currClubKvp.Key.Date) > 0 &&
                    _date.Date.CompareTo(interestedClubKvp.Key.Date) <= 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                
            }
            return false;

        }
        public void SetInterestedClub(TMClub club)
        {
            _interestedClub = club;
        }

        public void DetermineCurrentClub()
        {
            //var keysCollection = Player.TransferDictionary.Keys;

            var playerTransfers = _player.GetTransferDictionary();
            /* Loop on the SORTED dictionary till it finds a date that is after the given rumor source date,
             * if no transfers (like in /emmanual-sunday/profil/spieler/156476) return
             * Get the club related to the date BEFORE the one it found
             * If nothing before, current club is null
             * If nothing after, current club is last club
            */
            if (playerTransfers.Count <= 0) return;
            var prevKey = DateTime.MinValue;
            var prevprevKey = DateTime.MinValue;

            foreach (var kvp in playerTransfers)
            {
                if (kvp.Key.Date.CompareTo(_date.Date) >= 0)
                {
                    var daysDiff = (_date - prevKey).TotalDays;

                    _currentClub = daysDiff >= 10 ? playerTransfers[prevKey] : playerTransfers[prevprevKey];
                    return;
                }
                prevprevKey = prevKey;
                prevKey = kvp.Key;
            }
            _currentClub = playerTransfers[prevKey];
        }
    }
}