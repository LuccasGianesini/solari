using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Solari.Ganymede.Framework
{
    //Taken from some dude on stackoverflow!!!!!
    public static class UrlDecoder
    {
        public static JObject Decode(string input)
        {
            var obj = new JObject();

            string[] items = input.Replace("+", " ").Split('&');

            // Iterate over all name=value pairs.
            foreach (string item in items)
            {
                string[] param = item.Split('=');
                string key = HttpUtility.UrlDecode(param[0]);

                if (!string.IsNullOrEmpty(key))
                {
                    // If key is more complex than 'foo', like 'a[]' or 'a[b][c]', split it
                    // into its component parts.
                    string[] keys = key.Split(new[] {"]["}, StringSplitOptions.RemoveEmptyEntries);
                    int keysLast = keys.Length - 1;

                    // If the first keys part contains [ and the last ends with ], then []
                    // are correctly balanced.
                    if (Regex.IsMatch(keys[0], @"\[") && Regex.IsMatch(keys[keysLast], @"\]$"))
                    {
                        // Remove the trailing ] from the last keys part.
                        keys[keysLast] = Regex.Replace(keys[keysLast], @"\]$", string.Empty);

                        // Split first keys part into two parts on the [ and add them back onto
                        // the beginning of the keys array.
                        keys = keys[0].Split(new[] {'['}).Concat(keys.Skip(1)).ToArray();
                        keysLast = keys.Length - 1;
                    }
                    else
                    {
                        // Basic 'foo' style key.
                        keysLast = 0;
                    }

                    // Are we dealing with a name=value pair, or just a name?
                    if (param.Length == 2)
                    {
                        string val = HttpUtility.UrlDecode(param[1]);

                        // Coerce values.
                        // Convert val to int, double, bool, string
                        if (keysLast != 0)
                        {
                            // Complex key, build deep object structure based on a few rules:
                            // * The 'cur' pointer starts at the object top-level.
                            // * [] = array push (n is set to array length), [n] = array if n is 
                            //   numeric, otherwise object.
                            // * If at the last keys part, set the value.
                            // * For each keys part, if the current level is undefined create an
                            //   object or array based on the type of the next keys part.
                            // * Move the 'cur' pointer to the next level.
                            // * Rinse & repeat.
                            object cur = obj;

                            for (var i = 0; i <= keysLast; i++)
                            {
                                int index = -1;

                                // Array 'a[]' or 'a[1]', 'a[2]'
                                key = keys[i];

                                if (key == string.Empty || int.TryParse(key, out index)) key = index == -1 ? "0" : index.ToString(CultureInfo.InvariantCulture);

                                switch (cur)
                                {
                                    case JArray jarr when i == keysLast:
                                    {
                                        if (index >= 0 && index < jarr.Count)
                                            jarr[index] = val;
                                        else
                                            jarr.Add(val);

                                        break;
                                    }

                                    case JArray jarr:
                                    {
                                        if (index < 0 || index >= jarr.Count)
                                        {
                                            if (keys[i + 1] == string.Empty || int.TryParse(keys[i + 1], out int _))
                                                jarr.Add(new JArray());
                                            else
                                                jarr.Add(new JObject());

                                            index = jarr.Count - 1;
                                        }

                                        cur = jarr.ElementAt(index);

                                        break;
                                    }

                                    case JObject jobj when i == keysLast:
                                        jobj[key] = val;

                                        break;

                                    case JObject jobj:
                                    {
                                        if (jobj[key] == null)
                                        {
                                            if (keys[i + 1] == string.Empty || int.TryParse(keys[i + 1], out int _))
                                                jobj.Add(key, new JArray());
                                            else
                                                jobj.Add(key, new JObject());
                                        }

                                        cur = jobj[key];

                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Simple key, even simpler rules, since only scalars and shallow
                            // arrays are allowed.
                            if (obj[key] is JArray)

                                // val is already an array, so push on the next value.
                                ((JArray) obj[key]).Add(val);
                            else if (obj[key] != null && val != null)

                                // val isn't an array, but since a second value has been specified,
                                // convert val into an array.
                                obj[key] = new JArray {obj[key], val};
                            else

                                // val is a scalar.
                                obj[key] = val;
                        }
                    }
                    else if (!string.IsNullOrEmpty(key))
                    {
                        // No value was defined, so set something meaningful.
                        obj[key] = null;
                    }
                }
            }

            return obj;
        }
    }
}