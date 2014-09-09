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

        public TMClub(string id, string name, string shortName = null)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public bool ContainsName(string text)
        {
            var textToLower = text.ToLower();
            return textToLower.Contains(Name.ToLower()) || (ShortName != null && textToLower.Contains(ShortName.ToLower()));
        }
    }
    
}