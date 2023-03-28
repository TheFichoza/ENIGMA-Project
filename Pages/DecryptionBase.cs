using Microsoft.AspNetCore.Components;

namespace ENIGMA_Project.Pages
{
    public class DecryptionBase : ComponentBase
    {
        
        public static int[,] vars = { { 1, 2, 3 }, { 1, 2, 4 }, { 1, 2, 5 }, { 1, 3, 2 }, { 1, 3, 4 },
        { 1, 3, 5 }, { 1, 4, 2 }, { 1, 4, 3 }, { 1, 4, 5 }, { 1, 5, 2 }, { 1, 5, 3 }, { 1, 5, 4 },
        { 2, 1, 3 }, { 2, 1, 4 }, { 2, 1, 5 }, { 2, 3, 1 }, { 2, 3, 4 }, { 2, 3, 5 }, { 2, 4, 1 },
        { 2, 4, 3 }, { 2, 4, 5 }, { 2, 5, 1 }, { 2, 5, 3 }, { 2, 5, 4 }, { 3, 1, 2 }, { 3, 1, 4 },
        { 3, 1, 5 }, { 3, 2, 1 }, { 3, 2, 4 }, { 3, 2, 5 }, { 3, 4, 1 }, { 3, 4, 2 }, { 3, 4, 5 },
        { 3, 5, 1 }, { 3, 5, 2 }, { 3, 5, 4 }, { 4, 1, 2 }, { 4, 1, 3 }, { 4, 1, 5 }, { 4, 2, 1 },
        { 4, 2, 3 }, { 4, 2, 5 }, { 4, 3, 1 }, { 4, 3, 2 }, { 4, 3, 5 }, { 4, 5, 1 }, { 4, 5, 2 },
        { 4, 5, 3 }, { 5, 1, 2 }, { 5, 1, 3 }, { 5, 1, 4 }, { 5, 2, 1 }, { 5, 2, 3 }, { 5, 2, 4 },
        { 5, 3, 1 }, { 5, 3, 2 }, { 5, 3, 4 }, { 5, 4, 1 }, { 5, 4, 2 }, { 5, 4, 3 } };
        Dictionary<string, decimal> quad;
        private string[] fileLines;
        Key min;
        decimal hillClimb;
        string format;
        public void SetFileLines(string fileLine)
        {
            fileLines = fileLine.Split('\n');
            System.Console.WriteLine(fileLines.Length);
        }
        public string Decrypt(string cipherText)
        {
            format = "";
            char decSym;
            min = new Key();
            Key temp;
            Enigma enigma;
            int[] histogram = new int[26];
            foreach (char sym in cipherText)
            {
                if (sym > 96 && sym < 123) format += (char)(sym - 32);
                else if (sym > 64 && sym < 91) format += sym;
            }
            
            for(int rotorConfigs=0;rotorConfigs<60;rotorConfigs++)
            {
                int[] rotor_config = { vars[rotorConfigs, 0], vars[rotorConfigs, 1], vars[rotorConfigs, 2] };
                enigma = new Enigma(rotor_config);
                for (int test3 = 0; test3 < 26; test3++)
                {
                    int[] positions = new int[3];
                    positions[2] = test3;
                    for (int test2 = 0; test2 < 26; test2++)
                    {
                        positions[1] = test2;
                        for (int test1 = 0; test1 < 26; test1++)
                        {
                            positions[0] = test1;
                            enigma.SetPositons(positions);
                            foreach (char sym in format)
                            {
                                decSym = enigma.Encrypt(sym);
                                histogram[decSym - 65]++;
                            }
                            temp = new Key(rotor_config, positions, histogram);
                            if (temp.ioc > min.ioc) { min = temp; System.Console.WriteLine(min.ToString());}
                            histogram = new int[26];
                        }
                    }
                }
            }
            enigma = new Enigma(min);
            foreach (var item in format)
            {
                min.text += enigma.Encrypt(item);
            }
            return $"BEST - {DiscoverPlugs()}";
        }


        private string DiscoverPlugs()
        {
            Enigma enigma;
            quad = new Dictionary<string, decimal>();
            foreach (string line in fileLines)
            {
                string[] a = line.Split(' ');
                quad[a[0]] = decimal.Parse(a[1]);
            }
            min.hillClimb = HillClimb(min.text);

                for (char i = 'A'; i <= 'Z'; i++)
                {
                    for (char j = (char)(i+1); j <= 'Z'; j++)
                    {
                        enigma = new Enigma(min);
                        string text = "";
                        if (enigma.plugboard.AddPlug($"{i}-{j}"))
                        {
                            foreach (var item in format)
                            {
                                text += enigma.Encrypt(item);
                            }
                            hillClimb = HillClimb(text);
                            if (hillClimb > min.hillClimb)
                            {
                                min.hillClimb = hillClimb;
                                min.plugboard.Add($"{i}-{j}");
                                min.text = text;
                            }
                            enigma.plugboard.RemoveLastPlug();
                        }
                    }
                }
            return min.ToString();
        }

        public decimal HillClimb(string text)
        {
            decimal Climb = 0;
            for (int k = 0; k < text.Length - 4; k++)
            {
                string four = text.Substring(k, 4);
                if (quad.ContainsKey(four)) Climb += quad[four];
                else Climb += -6;
            }
            return Climb;
        }
    }
}