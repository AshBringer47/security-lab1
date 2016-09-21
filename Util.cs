﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace security_lab1_csharp
{
    class Util
    {
        public enum Evristiks
        {
            COUNT,
            XI2,
            MAGIC,
            ENTROPY
        }
        public static Random random = new Random();
        public static string[] ngramFileNames = { "english_1grams.gram", "english_2grams.gram", "english_3grams.gram", "english_4grams.gram", "english_5grams.gram" };
        public static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string sortedalphabet = "ETAOINSRHLDCUMFGPWYBVKJXZQ";
        public static long recordNumber = 4372870785;
        public static Dictionary<string, double>[] theorNGramFreqs = new Dictionary<string, double>[5];
        public static Dictionary<string, long>[] theorNGramCounts = new Dictionary<string, long>[5];
        public static long[] theorNGramTopCounts = new long[5];

        public static Dictionary<string, double> getTheorNGramFrequency(int method)
        {
            if (theorNGramFreqs[method] == null)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();
                string[] lines = System.IO.File.ReadAllLines(ngramFileNames[method]);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(' ');
                    dictionary.Add(parts[0], Double.Parse(parts[1]) / recordNumber);
                }
                theorNGramFreqs[method] = dictionary;
            }
            return theorNGramFreqs[method];
        }
        public static Dictionary<string, long> getTheorNGramCounts(int method)
        {
            if (theorNGramCounts[method] == null)
            {
                Dictionary<string, long> dictionary = new Dictionary<string, long>();
                string[] lines = System.IO.File.ReadAllLines(ngramFileNames[method]);
                theorNGramTopCounts[method] = long.Parse(lines[0].Split(' ')[1]);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(' ');
                    dictionary.Add(parts[0], long.Parse(parts[1]));
                }
                theorNGramCounts[method] = dictionary;
            }
            return theorNGramCounts[method];
        }
        public static long getTheorTopCount(int method)
        {
            if (theorNGramTopCounts[method] == 0)
            {
                string[] lines = System.IO.File.ReadAllLines(ngramFileNames[method]);
                theorNGramTopCounts[method] = long.Parse(lines[0].Split(' ')[1]);
            }
            return theorNGramTopCounts[method];
        }
        public static Dictionary<string, double> getRealNGramFrequency(string data, int method)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            for (int i = 0; i < data.Length - method; i++)
            {
                string ngram = data.Substring(i, method + 1);
                if (!dictionary.ContainsKey(ngram))
                    dictionary.Add(ngram, ((double)Regex.Matches(data, ngram).Count) / data.Length);
            }
            for (int i = 0; i < alphabet.Length; i++)
            {
                if (!dictionary.ContainsKey(alphabet[i] + ""))
                {
                    dictionary.Add(alphabet[i] + "", 0);
                }
            }
            return dictionary;
        }
        public static Dictionary<string, long> getRealNGramCount(string data, int method)
        {
            Dictionary<string, long> dictionary = new Dictionary<string, long>();
            for (int i = 0; i < data.Length - method; i++)
            {
                string ngram = data.Substring(i, method + 1);
                if (!dictionary.ContainsKey(ngram))
                    dictionary.Add(ngram, ((long)Regex.Matches(data, ngram).Count));
            }
            for (int i = 0; i < alphabet.Length; i++)
            {
                if (!dictionary.ContainsKey(alphabet[i] + ""))
                {
                    dictionary.Add(alphabet[i] + "", 0);
                }
            }
            return dictionary;
        }
        public static List<KeyValuePair<string, long>> getSortedRealNGramCountList(string data, int method)
        {
            List<KeyValuePair<string, long>> list = Util.getRealNGramCount(data, method).ToList();
            list.Sort(
                delegate (KeyValuePair<string, long> pair1,
                KeyValuePair<string, long> pair2)
                {
                    return pair2.Value.CompareTo(pair1.Value);
                });
            return list;
        }
        public static List<KeyValuePair<string, double>> getSortedRealNGramFrequencyList(string data, int method)
        {
            List<KeyValuePair<string, double>> list = Util.getRealNGramFrequency(data, method).ToList();
            list.Sort(
                delegate (KeyValuePair<string, double> pair1,
                KeyValuePair<string, double> pair2)
                {
                    return pair2.Value.CompareTo(pair1.Value);
                });
            return list;
        }
        public static int gcd(int m, int n)
        {
            if (m % n == 0)
                return n;
            if (n % m == 0)
                return m;
            if (m > n)
                return gcd(m % n, n);
            if (m < n)
                return gcd(n % m, m);
            return 1;
        }
    }
}
