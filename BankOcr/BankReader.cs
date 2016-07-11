using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankOcr
{
    public class BankReader
    {
        const string zeroValue =
                " _ " +
                "| |" +
                "|_|";
        const string oneValue =
                "   " +
                "  |" +
                "  |";
        const string twoValue =
                " _ " +
                " _|" +
                "|_ ";
        const string threeValue =
                " _ " +
                " _|" +
                " _|";
        const string fourValue =
                "   " +
                "|_|" +
                "  |";
        const string fiveValue =
                " _ " +
                "|_ " +
                " _|";
        const string sixValue =
                " _ " +
                "|_ " +
                "|_|";
        const string sevenValue =
                " _ " +
                "  |" +
                "  |";
        const string eightValue =
                " _ " +
                "|_|" +
                "|_|";
        const string nineValue =
                " _ " +
                "|_|" +
                " _|";

        private readonly Dictionary<string, int> references = new Dictionary<string, int>
            {
                { zeroValue, 0},
                { oneValue , 1},
                { twoValue, 2 },
                { threeValue, 3 },
                { fourValue, 4 },
                { fiveValue, 5 },
                { sixValue, 6 },
                { sevenValue, 7 },
                { eightValue, 8 },
                { nineValue, 9 }
            };

        public string ReadNumbers(string value)
        {
            if (value.Length != 81)
            {
                throw new Exception();
            }
            var nbChiffre = value.Length / 9;

            var numbersAsString = new string[nbChiffre];
            var concatNumbers = new StringBuilder();

            for (var lineNumber = 0; lineNumber < 3; lineNumber++)
            {
                for (var i = 0; i < nbChiffre * 3; i += 3)
                {
                    if (numbersAsString[i / 3] == null)
                    {
                        numbersAsString[i / 3] = string.Empty;
                    }
                    numbersAsString[i / 3] += value.Substring(lineNumber * (nbChiffre * 3) + i, 3);
                }
            }
            
            List<List<int>> possibilities = new List<List<int>>(9);

            bool isNumberNotRecognized = false;
            foreach (var chiffre in numbersAsString)
            {
                possibilities.Add(this.GetPossibilities(chiffre));
                if (this.references.ContainsKey(chiffre))
                {
                    concatNumbers.Append(this.references[chiffre]);
                }
                else
                {
                    isNumberNotRecognized = true;
                    concatNumbers.Append("?");
                }
            }

            // Remplacer par les possibilités jusqu'a ce que le checksum soit bon.
            if (isNumberNotRecognized)
            {
                return concatNumbers.Append(" ILL").ToString();
            }

            var stringRepresentation = concatNumbers.ToString();

            return this.CheckAccount(Convert.ToInt32(stringRepresentation)) ? stringRepresentation : stringRepresentation + " ERR";
        }

        public bool CheckAccount(int entry)
        {
            int calcul = 0;

            var accountAsString = entry.ToString("000000000");
            int index = 1;

            for (var i = 9; i >= 1; i--)
            {
                calcul += Convert.ToInt32(accountAsString[i - 1].ToString()) * index;
                index++;
            }

            return calcul % 11 == 0;
        }

        private List<int> GetPossibilities(string entry)
        {
            var keys = this.references.Select(k => k.Key).ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (entry[j] != ' ' && keys[i][j] == ' ')
                    {
                        keys.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            
            Dictionary<string, int> differenceCounts = new Dictionary<string, int>(keys.Count);
            foreach (var key in keys)
            {
                differenceCounts[key] = 0;
                for (int j = 0; j < 9; j++)
                {
                    if (entry[j] != key[j])
                    {
                        differenceCounts[key]++;
                    }
                }
            }

            var listInt = differenceCounts.OrderBy(kvp => kvp.Value).Select(kvp => this.references[kvp.Key]).ToList().ToList();

            return listInt;
        } 
    }
}
