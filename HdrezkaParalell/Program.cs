using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace MyApp
{
    internal class Program
    {
        public static int i = 0;
        public static async Task Main(string[] args)
        {
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseCookies = true;
            httpClientHandler.UseDefaultCredentials = false;
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            Program program = new Program();
            // string url = "https://rezka.ag/animation/page/1/";
            //await program.MainParserForNames(url, httpClientHandler, userAgent);

            string someurl = Console.ReadLine();
            someurl = "https://rezka.ag/search/?do=search&subaction=search&q=" + someurl;
            //string someurl = "https://rezka.ag/animation/fantasy/56646-reyting-korolya-sunduk-hrabrosti-2023.html";
        //    await GoToPageParser(someurl, httpClientHandler, userAgent);
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
                        myList.Add(new Tuple<int, string, string >(i, link.InnerText, href));
                        i++;
                    }
                }
                //int k = 0;
                foreach (var item in myList)
                {
                    Console.WriteLine("ID: {0}\n Name: {1}\n Link: {2}\n", item.Item1, item.Item2, item.Item3);
                    //await GoToPageParser(myList[k].Item3, httpClientHandler, userAgent);
                  // await GoToPageParser(item.Item3, httpClientHandler, userAgent);
                }
            }
        }

        public static async Task GoToPageParser(string url5, HttpClientHandler httpClientHandler, string userAgent)
        {
            var url = url5;

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

                    var response = await httpClient.GetAsync(url);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var html = responseBody;

                    var doc2 = new HtmlDocument();
                    doc2.LoadHtml(html);

                    var diva = doc2.DocumentNode.SelectNodes("//td[@class='l']");
                    var diva2 = doc2.DocumentNode.SelectNodes("//td");


                    List<KeyValuePair<string, string>> ratelist = new List<KeyValuePair<string, string>>();


                    for (int i = 0; i < diva2.Count; i++)
                    {
                       // Console.WriteLine(diva[i].InnerText);
                        Console.Write(diva2[i].InnerText);
                        Console.Write("\n\n");

                    }

                    //foreach (var item in diva)
                    //   {

                    //       Console.WriteLine(item.InnerText);
                    //   }
                    //   foreach (var item in diva2)
                    //   {
                    //       Console.WriteLine(item.InnerText);

                    //   }

                }
                catch (Exception)
                {
                    Console.WriteLine("err");
                }
               
            }
        }
        }
}
      
