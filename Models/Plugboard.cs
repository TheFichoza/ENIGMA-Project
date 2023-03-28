using System.Text.RegularExpressions;
namespace ENIGMA_Project
{
 internal class Plugboard
    {
        HashSet<int> usedChars;
        Dictionary<int, int> maps;
        public Plugboard()
        {
            usedChars = new HashSet<int>();
            maps = new Dictionary<int, int>();
        }
        //changed to IENumberable for greater flexibility
        public Plugboard(IEnumerable<string> plugs)
        {
            usedChars = new HashSet<int>();
            maps = new Dictionary<int, int>();
            foreach (string plug in plugs)
            {
                AddPlug(plug);
            }
        }
        public bool AddPlug(string conn)
        {
            if (Regex.IsMatch(conn, "^[A-Z]-[A-Z]$"))
            {
                int symbol1 = conn[0] - 65,
                symbol2 = conn[2] - 65;
                if (!(usedChars.Contains(symbol1) || usedChars.Contains(symbol2)))
                {
                    usedChars.Add(symbol1);
                    usedChars.Add(symbol2);
                    maps[symbol1] = symbol2;
                    return true;
                }
            }
            return false;
        }
        public void RemoveLastPlug()
        {
            usedChars.Remove(maps.LastOrDefault().Key);
            usedChars.Remove(maps.LastOrDefault().Value);
            maps.Remove(maps.LastOrDefault().Key);
        }
        public int Connect(int symbol)
        {
            if (usedChars.Contains(symbol))
            {
                if (maps.Values.Contains(symbol))
                {
                    return maps.FirstOrDefault(x => x.Value == symbol).Key;
                }
                else
                {
                    return maps[symbol];
                }
            }
            return symbol;
        }
        public override string ToString()
        {
            string s = "";
            foreach (KeyValuePair<int, int> pair in maps)
            {
                s += $"{(char)(pair.Key + 65)}-{(char)(pair.Value + 65)} ";
            }
            return s;
        }
    }   
}