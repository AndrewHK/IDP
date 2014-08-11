/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:26 PM
 * 
*/

using System;
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

            //DetermineCurrentClub();
        }

        public string RumorId { get; set; }
        public string Headline { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }

        public TMPlayer Player { get; set; }
        public TMClub CurrentClub { get; set; }



        public override string ToString()
        {
            return string.Format("[TMRumorSource RumorID={0}, Headline={1}, Date={2}, Name={3}, URL={4}, Text={5}]",
                RumorId, Headline, Date, Name, Url, Text);
        }

        public void DetermineCurrentClub()
        {
            //var keysCollection = Player.TransferDictionary.Keys;
            //var sourceDate = Convert.ToDateTime(Date);
            //TODO figure out a way to find where the rumor source date is situated
        }
    }
}