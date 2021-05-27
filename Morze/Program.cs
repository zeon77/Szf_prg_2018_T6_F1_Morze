using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace Morze
{
    class Morze
    {
        //5.
        public static Dictionary<string, string> MorzeABC = new Dictionary<string, string>();
        public static Dictionary<string, string> ABCMorze = new Dictionary<string, string>();

        public static bool Initialize(string filepath)
        {
            try
            {
                foreach (var sor in File.ReadAllLines(filepath).Skip(1))
                {
                    string[] m = sor.Split('\t');
                    ABCMorze.Add(m[0], m[1]);
                    MorzeABC.Add(m[1], m[0]);
                }
                MorzeABC.Add("   ", "");
                MorzeABC.Add("       ", " ");

                ABCMorze.Add("", "   ");
                ABCMorze.Add(" ", "       ");
                return true;
            }
            catch
            {
                return false;
            }
        } 

        //6.
        public static string Morze2Szöveg(string morzeKód)
        {
            //Itt nagyon fontos, hogy a 7 szóköz legyen előbb!!!
            Regex r = new Regex("[.-]+| {7}| {3}");
            var matches = r.Matches(morzeKód).Cast<Match>().Select(m => m.Value).ToList();
            return string.Join("", matches.Select(m => MorzeABC.First(x => x.Key == m).Value));
        }

        public static string Szöveg2Morze(string Szöveg)
        {
            throw new NotImplementedException();
        }
    }

    class Idézet
    {
        public string MorzeSzerző { get; set; }
        public string MorzeIdézet { get; set; }
        public string HUNSzerző { get; set; }
        public string HUNIdézet { get; set; }

        public Idézet(string sor)
        {
            string[] s = sor.Split(';');
            MorzeSzerző = s[0];
            HUNSzerző = Morze.Morze2Szöveg(s[0]);
            MorzeIdézet = s[1];
            HUNIdézet = Morze.Morze2Szöveg(s[1]);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Morze.Initialize("morzeabc.txt");

            //3.
            Console.WriteLine($"3. feladat: A morze ABC {Morze.MorzeABC.Count} db karakter kódját tartalmazza");

            //4.
            Console.Write($"4. feladat: Kérek egy karaktert: ");
            string ch = Console.ReadLine().ToUpper();

            //if (!Morze.MorzeABC.Any(x => x.Value == ch))
            if (!Morze.ABCMorze.ContainsKey(ch))
                Console.WriteLine("Nem található a kódtárban ilyen karakter!");
            else
                //Console.WriteLine($"\t A(z) {ch} karakter morze kódja: {Morze.MorzeABC.First(x => x.Value == ch).Key}");
                Console.WriteLine($"\t A(z) {ch} karakter morze kódja: {Morze.ABCMorze[ch]}");

            //5.
            List<Idézet> idézetek = new List<Idézet>();
            foreach (var sor in File.ReadAllLines("morze.txt"))
            {
                idézetek.Add(new Idézet(sor));
            }

            //7.
            Console.WriteLine($"7. feladat: Az első idézet szerzője: {idézetek[0].HUNSzerző}");

            //8.
            var i = idézetek.OrderBy(x => x.HUNIdézet.Length).Last();
            Console.WriteLine($"8. feladat: A leghosszabb szerzője és az idézet: {i.HUNSzerző}: {i.HUNIdézet}");

            //9.
            Console.WriteLine($"9. feladat: Arisztotelész idézetei:");
            idézetek
                .Where(x => x.HUNSzerző == "Arisztotelész".ToUpper())
                .ToList()
                .ForEach(x => Console.WriteLine($"\t - {x.HUNIdézet}"));

            //10.
            File.WriteAllLines("forditas.txt", idézetek.Select(x => x.HUNSzerző + ":" + x.HUNIdézet).ToArray());

            Console.ReadKey();
        }
    }
}
