/*
 * Created by SharpDevelop.
 * User: Andrew
 * Date: 18-Jul-14
 * Time: 1:24 PM
 * 
*/

using System.Collections.Generic;
using IDPParser.Model;

namespace IDPParser.Control
{
    /// <summary>
    ///     Description of ExtensionMethods.
    /// </summary>
    public class CustomList : List<TMRumorSource>
    {
        public CustomList() : base()
        {
        }

        public override string ToString()
        {
            return ToArray().ToString();
        }
    }
}