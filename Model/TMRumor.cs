/*
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
        private readonly IList<TMRumorSource> _sourcesList;

        public TMRumor(TMRumor copyRumor)
        {
            Id = copyRumor.Id;
            Url = copyRumor.Url;
            NoOfPages = copyRumor.NoOfPages;
            Player = copyRumor.Player;
            CurrentClub = copyRumor.CurrentClub;
            InterestedClub = copyRumor.InterestedClub;
            Type = copyRumor.Type;
            _sourcesList = new List<TMRumorSource>();
        }

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
        
        public TMPlayer Player { get; set; }
        public TMClub CurrentClub { get; set; }
        public TMClub InterestedClub { get; set; }
        public TMRumorType Type { get; set; }
        
        public int NoOfPages { get; set; }
        public int NoOfSources { get; set; }
        public bool IsParsed { get; set; }
        public bool FixedSourceExists { get; set; }


        public IList<TMRumorSource> GetSources()
        {
            return _sourcesList;
        }

        public void AddSource(TMRumorSource source)
        {
            _sourcesList.Add(source);
            NoOfSources++;
        }
    }
}