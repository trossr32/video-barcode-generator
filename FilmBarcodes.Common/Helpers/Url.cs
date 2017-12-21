using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmBarcodes.Common.Helpers
{
    public static class Url
    {
        public static string StringifyParametersDictionary(this Dictionary<string, string> parameters)
        {
            return parameters == null
                ? ""
                : parameters.Aggregate(new StringBuilder(), (sb, kvp) => sb.AppendFormat("&{0}={1}", kvp.Key, kvp.Value), sb => sb.ToString());
        }
    }
}