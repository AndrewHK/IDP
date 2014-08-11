/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:25 PM
 * 
*/

using System;
using System.Collections.Generic;

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

            TransferDictionary = new SortedDictionary<DateTime, TMClub>();
        }

        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Height { get; set; }
        public string Position { get; set; }
        public string Nationality { get; set; }
        public string StrongerFoot { get; set; }
        public IDictionary<DateTime, TMClub> TransferDictionary { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "[TMPlayer ID={0}, URL={1}, Name={2}, DateOfBirth={3}, Height={4}, Position={5}, Nationality={6}, StrongerFoot={7}]",
                    Id, Url, Name, DateOfBirth, Height, Position, Nationality, StrongerFoot);
        }

        public void AddTransfer(String dateStr, TMClub club)
        {
            var date = Convert.ToDateTime(dateStr);
            TransferDictionary.Add(date,club);
        }
    }
}