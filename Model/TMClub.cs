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


        public override string ToString()
        {
            return string.Format("[TMClub ID={0}, URL={1}, Name={2}]", Id, Url, Name);
        }
    }
}