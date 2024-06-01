using System.Collections.Generic;

namespace NetCore8583.Util
{
    /// <summary>
    /// </summary>
    public static class DictionaryExtensions
    {
        public static void AddAll<TK, TV>(this Dictionary<TK, TV> dic,
            Dictionary<TK, TV> val)
        {
            foreach (var kvp in val)
            {
                dic.Add(kvp.Key, kvp.Value);
            }
        }
    }
}