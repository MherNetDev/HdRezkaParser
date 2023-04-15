using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyApp
{
    internal class Program
    {
        private static async Task Main()
        {
            var httpClientFactory = new HttpClientFactory();
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, "output.txt");
            var myList = new ConcurrentBag<Tuple<int, string, string>>();

            Console.WriteLine("1: for all names\n2: for search");
            byte choosed = Convert.ToByte(Console.ReadLine());

            if (choosed == 1)
            {
                Console.WriteLine("All names Parsing");
                var httpClient = httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

                await Task.WhenAll(Enumerable.Range(1, 1053).Select(async j =>
                {
                    var url = $"https://rezka.ag/page/{j}/";
                    var response = await httpClient.GetAsync(url);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(responseBody);

                    var links = doc.DocumentNode.SelectNodes("//*[@class='b-content__inline_item-link']//a[@href]");
                    if (links != null)
                    {
                        foreach (var link in links)
                        {
                            var href = link.GetAttributeValue("href", "");
                            myList.Add(new Tuple<int, string, string>(myList.Count + 1, link.InnerText, href));
                        }
                    }
                }));
            }
            else if (choosed == 2)
            {
                Console.WriteLine("Type Searched name");
                var searchName = Console.ReadLine();
                var url = $"https://rezka.ag/search/?do=search&subaction=search&q={searchName}";
                var httpClient = httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                var response = await httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(responseBody);

                var links = doc.DocumentNode.SelectNodes("//*[@class='b-content__inline_item-link']//a[@href]");
                if (links != null)
                {
                    foreach (var link in links)
                    {
                        var href = link.GetAttributeValue("href", "");
                        myList.Add(new Tuple<int, string, string>(myList.Count + 1, link.InnerText, href));
                    }
                }
            }
            else
            {
                Console.WriteLine("wtf");
            }

            using (var writer = new StreamWriter(filePath, append: true))
            {
                Parallel.ForEach(myList, item =>
                {
                    Console.WriteLine("ID: {0}\n Name: {1}\n Link: {2}\n", item.Item1, item.Item2, item.Item3);
                    lock (writer)
                    {
                        writer.WriteLine("ID: {0}\n Name: {1}\n Link: {2}\n", item.Item1, item.Item2, item.Item3);
                    }
                });
            }







        }
    }
}
