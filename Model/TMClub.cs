/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 23-Jun-14
 * Time: 3:25 PM
 * 
*/

namespace IDPParser.Model
{
    /// <summary>
    ///     Description of TMClub.
    /// </summary>
    public class TMClub
    {
        public TMClub()
        {
        }

        public TMClub(string id, string url, string name)
        {
            Id = id;
            Url = url;
            Name = name;
        }

        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
}