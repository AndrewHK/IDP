/*
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:15 PM
 * 
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            Title = copyRumor.Title;
            Url = copyRumor.Url;
            Player = copyRumor.Player;
            IsSuccessful = copyRumor.IsSuccessful;
            Type = copyRumor.Type;
            _sourcesList = new List<TMRumorSource>();
        }

        public TMRumor(string id, string title, string url, int noOfPages)
        {
            Id = id;
            Title = title;
            Url = url;
            NoOfPages = noOfPages;
            Player = new TMPlayer();
            IsSuccessful = false;
            _sourcesList = new List<TMRumorSource>();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public TMPlayer Player { get; set; }
        public bool IsSuccessful { get; set; }

        public string Type { get; set; }

        public int NoOfPages { get; set; }
        public int NoOfSources { get; set; }
        public bool IsParsed { get; set; }
        public bool IsFixedSourceExist { get; set; }


        public IList<TMRumorSource> GetSources()
        {
            return _sourcesList;
        }

        public void AddSource(TMRumorSource source)
        {
            _sourcesList.Add(source);
            NoOfSources++;
        }

        public bool RemoveSource(TMRumorSource source)
        {
            if (_sourcesList.Remove(source))
            {
                NoOfSources--;
                return true;
            }
            return false;
        }
    }
}