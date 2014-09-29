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
using System.Net.NetworkInformation;

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
            _rumorSourceDate = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
            
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
                return _rumorSourceDate.ToShortDateString();
            }
        }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }

        

        private DateTime _rumorSourceDate;
        private DateTime _currentClubDate;
        private TMClub _currentClub;
        private TMClub _interestedClub;
        private TMPlayer _player;
        private const string DateFormat = "dd.MM.yyyy - HH:mm";
        private string _type;
        

        public void SetPlayer(TMPlayer player)
        {
            _player = player;
        }

        public String GetRumorType()
        {
            return _type;
        }

        public bool IsRumorSuccessful()
        {
            var playerTransfers = _player.GetTransferDictionary();
            var playerLoans = _player.GetLoanDictionary();
            try
            {
                var interestedClubTransfersKvp = playerTransfers.FirstOrDefault(x => x.Value.Id.Equals(InterestedClubId) && x.Key.Date.CompareTo(_rumorSourceDate.Date) >= 0);

                if (interestedClubTransfersKvp.Value != null && !playerTransfers.Any(x => x.Key.Date.CompareTo(_currentClubDate.Date) > 0 && x.Key.Date.CompareTo(interestedClubTransfersKvp.Key.Date) < 0))
                {
                    _type = "Transfer";
                    return true;
                }

                var interestedClubLoansKvp = playerLoans.FirstOrDefault(x => x.Value.Id.Equals(InterestedClubId) && x.Key.Date.CompareTo(_rumorSourceDate.Date) >= 0);

                if (interestedClubLoansKvp.Value != null && !playerTransfers.Any(x => x.Key.Date.CompareTo(_currentClubDate.Date) > 0 && x.Key.Date.CompareTo(interestedClubLoansKvp.Key.Date) < 0))
                {
                    _type = "Loan";
                    return true;
                }

            }
            catch
            {
                
            }
            return false;

        }

        public void SetInterestedClub(TMClub club)
        {
            _interestedClub = club;
        }

        public void DetermineCurrentClub(TMClub defaultClub)
        {

            var playerTransfers = _player.GetTransferDictionary();
            const int minDayDiff = 10;
            /* Loop on the SORTED dictionary till it finds a date that is after the given rumor source date,
             * if no transfers (like in /emmanual-sunday/profil/spieler/156476) return
             * Get the club related to the date BEFORE the one it found
             * If nothing before, current club is null
             * If nothing after, current club is last club
            */
            if (playerTransfers.Count <= 0)
            {
                _currentClub = defaultClub;
                return;
            }
            var prevKey = DateTime.MinValue;
            var prevprevKey = DateTime.MinValue;
            try
            {
                foreach (var kvp in playerTransfers)
                {
                    if (kvp.Key.Date.CompareTo(_rumorSourceDate.Date) >= 0)
                    {
                        var daysDiff = (_rumorSourceDate - prevKey).TotalDays;

                        if (daysDiff >= minDayDiff)
                        {
                            _currentClub = playerTransfers[prevKey];
                            _currentClubDate = prevKey;
                        }
                        else
                        {
                            _currentClub = playerTransfers[prevprevKey];
                            _currentClubDate = prevprevKey;
                        }
                         
                        
                        return;
                    }
                    prevprevKey = prevKey;
                    prevKey = kvp.Key;
                }
                _currentClub = playerTransfers[prevKey];
                _currentClubDate = prevKey;
            }
            catch
            {
            }
        }
    }
}