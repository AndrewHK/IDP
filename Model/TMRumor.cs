/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:15 PM
 * 
*/

using System.Collections.Generic;

namespace IDPParser.Model
{
    /// <summary>
    ///     Description of TMRumor.
    /// </summary>
    public class TMRumor
    {
        private readonly List<TMRumorSource> _sourcesList;

        public TMRumor(string id, string url, int noOfPages)
        {
            Id = id;
            Url = url;
            NoOfPages = noOfPages;
            Player = new TMPlayer();
            CurrentClub = new TMClub();
            InterestedClub = new TMClub();
            Type = TMRumorType.Undecided;
            _sourcesList = new List<TMRumorSource>();
        }

        public string Id { get; set; }
        public string Url { get; set; }
        public int NoOfPages { get; set; }

        public TMPlayer Player { get; set; }
        public TMClub CurrentClub { get; set; }
        public TMClub InterestedClub { get; set; }
        public TMRumorType Type { get; set; }
        public int NoOfSources { get; set; }

        public bool IsParsed { get; set; }
        public bool FixedSourceExists { get; set; }

        public void AddSource(TMRumorSource source)
        {
            _sourcesList.Add(source);
            NoOfSources++;
        }

        public override string ToString()
        {
            return
                string.Format(
                    "[TMRumor ID={0}, URL={1}, NoOfPages={2}, Player={3}, CurrentClub={4}, InterestedClub={5}, Type={6}, SourcesList Count={7}]",
                    Id, Url, NoOfPages, Player, CurrentClub, InterestedClub, Type, _sourcesList.Count);
        }
    }
}