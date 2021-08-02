using ConsoleTables;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Data;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Clear();
            Console.WriteLine("Argumentos Validos:");
            Console.WriteLine(" a - sitealtudo.co");
            Console.WriteLine(" b - sitegeneric");

            string entrada = Console.ReadKey().Key.ToString().ToLower();

            Console.Clear();

            Console.WriteLine(" Escolha uma opção acima");

            if (entrada == "a")
            {
                functionA();
            }
            else
            {
                functionB();

            }


        }

        private static void functionB()
        {
            Console.Clear();
            Console.WriteLine("Digite a URL do site");

            string url = Console.ReadLine();

            Console.Clear();

            Console.WriteLine(" Escolha uma opção acima");
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            Dictionary<string, DataRow> dcWord = new Dictionary<string, DataRow>();
            DataTable dt = new DataTable();
            dt.Columns.Add("word");
            dt.Columns.Add("count", typeof(int));
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//text()[normalize-space(.) != '']"))
            {
                foreach (string word in node.InnerText.Trim().Split(" "))
                {
                    string wordnew = StringHelper.SemCaracteresEspeciais(StringHelper.SemAcentos2(word.ToUpper())).Replace("\n","").Replace("\r","");
                    if (wordnew.StartsWith("&") || wordnew.Length > 15 || string.IsNullOrWhiteSpace(wordnew))
                    {
                        continue;
                    }
                    if (!dcWord.ContainsKey(wordnew))
                    {
                        DataRow dr = dt.NewRow();
                        dr["word"] = wordnew.ToUpper();
                        dr["count"] = 0;
                        dcWord.Add(wordnew, dr);
                        dt.Rows.Add(dr);
                    }
                    var count = Convert.ToInt32(dcWord[wordnew]["count"]);
                    dcWord[wordnew]["count"] = ++count;
                }

            }
            dt.DefaultView.Sort = "count DESC";
            var table = new ConsoleTable("Word", "Count");
            foreach (DataRow item in dt.DefaultView.ToTable().Rows)
            {
                table.AddRow(item["word"], item["count"]);
            }
            Console.WriteLine(table.ToString());
            Console.Read();
        }

        private static void functionA()
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            List<Integrant> team = new List<Integrant>();
            Dictionary<string, int> dcFirst = new Dictionary<string, int>();
            Dictionary<string, int> dcLast = new Dictionary<string, int>();

            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.altudo.co/why-altudo");

            List<Integrant> teamList = FormatHTML(doc, team);

            var table = new ConsoleTable("Firstname", "Lastname", "Designation", "repeatedFirstname", "repeatedLastname");

            foreach (Integrant item in teamList)
            {

                dcFirst = PopularDic(dcFirst, item.Firstname);
                dcLast = PopularDic(dcLast, item.Lastname);
            }
            foreach (Integrant item in teamList)
            {
                string firstName = item.Firstname.ToString();
                string lastName = item.Lastname.ToString();
                int countFirstname = 0;
                int countLastname = 0;

                if (dcFirst.ContainsKey(firstName))
                {
                    countFirstname = dcFirst[firstName];
                }

                if (dcLast.ContainsKey(lastName))
                {
                    countLastname = dcLast[lastName];
                }

                table.AddRow(item.Firstname, item.Lastname, item.Designation, countFirstname, countLastname);


            }

            Console.WriteLine(table.ToString());
            Console.Read();
        }

        private static Dictionary<string, int> PopularDic(Dictionary<string, int> dic, string item)
        {
            if (!dic.ContainsKey(item))
            {
                dic.Add(item, -1);
            }
            dic[item]++;

            return dic;
        }

        private static List<Integrant> FormatHTML(HtmlDocument doc, List<Integrant> team)
        {
            ////*[@id="testimonial-slider"]/div[1]/div/div[2]
            //var nodes = doc.DocumentNode.SelectNodes("//h2[@class='about-page-heading']");
            var nodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'testimonial col-lg-4 col-md-6 d-flex')]");
            foreach (var node in nodes)
            {
                var p = node.SelectSingleNode(".//p");
                var h2 = node.SelectSingleNode(".//h2");
                var span = node.SelectSingleNode(".//span");
                string[] name = h2.InnerText.ToString().Trim().Split(" ");
                var user = new Integrant
                {
                    Firstname = name[0],
                    Lastname = name[1],
                    Designation = span.InnerHtml,
                    Description = p.InnerHtml
                };

                team.Add(user);
            }
            return team;
        }
    }
}