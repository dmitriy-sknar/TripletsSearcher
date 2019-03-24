using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace TripletsSearcher {
    class Program {

        static void Main(string[] args) {
            string outPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "output.txt");

            if (args.Length != 0) {
                string path = args[0];
                int[] arr = parseAndSort(path);
                List<string> triplets = findTriplets(arr);
                saveResults(outPath, triplets);
            } else {
                Console.WriteLine("Path to input file is empty!");
            }
            Console.ReadKey();
        }

        public static int[] parseAndSort(string path) {
            List<int> numbers = new List<int>();
            try {
                TextFieldParser parser = new TextFieldParser(path);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData) {
                    string[] fields = parser.ReadFields();
                    foreach (string field in fields) {
                        try {
                            numbers.Add(Int32.Parse(field));
                        } catch (FormatException) {
                            Console.WriteLine($"Unable to parse '{field}'");
                        }
                    }
                }
            } catch (FileNotFoundException e) {
                Console.WriteLine("File was not found! Please ensure filename is correct");
                Console.ReadKey();
                Environment.Exit(1);
            }
            numbers.Sort();
            return numbers.ToArray();
        }

        public static List<string> findTriplets(int[] arr) {
            int n = arr.Length;
            bool found = false;
            HashSet<int> hashSet = new HashSet<int>();
            List<string> triplets = new List<string>();

            for (int i = 0; i < n - 1; i++) {
                // Find all pairs with sum equals to "-arr[i]" 
                hashSet.Clear();
                for (int j = i + 1; j < n; j++) {
                    int x = -(arr[i] + arr[j]);
                    if (arr[j - 1] == arr[j])
                        continue; // skip duplicate triplets. Works only if arr is sorted
                    if (hashSet.Contains(x)) {
                        triplets.Add($"{x}, {arr[i]}, {arr[j]}");
                        found = true;
                    } else
                        hashSet.Add(arr[j]);
                }
            }

            if (found == false)
                triplets.Add("No Triplet Found");
            return triplets;
        }

        public static void saveResults(string outPath, List<string> triplets) {
            StringBuilder outputStrBuilder = new StringBuilder();
            foreach (string triplet in triplets) {
                outputStrBuilder.Append(triplet + "\n");
            }

            using (StreamWriter sr = File.AppendText(outPath)) {
                foreach (string triplet in triplets) {
                    sr.WriteLine(triplet);
                }
                sr.Close();
            }
            if (triplets[0].Equals("No Triplet Found")) {
                Console.WriteLine(triplets[0]);
                return;
            }
            Console.WriteLine($"Done! Found {triplets.Count} triplets");
            Console.WriteLine($"Checkout '{outPath}' file");
        }
    }
}
