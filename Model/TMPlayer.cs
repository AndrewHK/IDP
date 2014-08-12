/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:25 PM
 * 
*/

using System;
using System.Collections.Generic;
using System.Globalization;

namespace IDPParser.Model
{
    /// <summary>
    ///     Description of TMPlayer.
    /// </summary>
    public class TMPlayer
    {
        public TMPlayer()
        {
        }

        public TMPlayer(string id, string url, string name, string dateOfBirth, string height, string position,
            string nationality, string strongerFoot)
        {
            Id = id;
            Url = url;
            Name = name;
            DateOfBirth = dateOfBirth;
            Height = height;
            Position = position;
            Nationality = nationality;
            StrongerFoot = strongerFoot;

            _transferDictionary = new SortedDictionary<DateTime, TMClub>();
        }

        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Height { get; set; }
        public string Position { get; set; }
        public string Nationality { get; set; }
        public string StrongerFoot { get; set; }

        private readonly IDictionary<DateTime, TMClub> _transferDictionary;

        private const string DateFormat = "dd.MM.yyyy";

        public IDictionary<DateTime, TMClub> GetTransferDictionary()
        {
            return _transferDictionary;
        }

        public void AddTransfer(String dateStr, TMClub club)
        {
            var date = DateTime.ParseExact(dateStr, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
            _transferDictionary.Add(date,club);
        }
    }
}