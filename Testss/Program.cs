using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using System.Diagnostics;

namespace MyApp
{
    internal class Program
    {
        public static int i = 0;
        public static async Task Main(string[] args)
        {
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            Stopwatch stopwatch = new Stopwatch();
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseCookies = true;
            httpClientHandler.UseDefaultCredentials = false;
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            Program program = new Program();
            string someurl = "https://rezka.ag/";
            await program.MainParserForNames(someurl, httpClientHandler, userAgent);

        }

        public async Task MainParserForNames(string url2, HttpClientHandler httpClientHandler, string userAgent)
        {
            var url = url2;

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                var response = await httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                var html = responseBody;
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var div = doc.DocumentNode.SelectNodes("//*[@class='b-content__inline_item-link']");
                var links = doc.DocumentNode.SelectNodes("//*[@class='b-content__inline_item-link']//a[@href]");

                var myList = new List<Tuple<int, string, string>>();

                if (links != null)
                {
                    foreach (var link in links)
                    {
                        var href = link.GetAttributeValue("href", "");
                        myList.Add(new Tuple<int, string, string>(i, link.InnerText, href));
                        i++;
                    }
                }
                foreach (var item in myList)
                    Console.WriteLine("ID: {0}\n Name: {1}\n Link: {2}\n", item.Item1, item.Item2, item.Item3);

            }
        }

    }
}

