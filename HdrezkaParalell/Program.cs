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
        public static int p = 0;
        public static int j = 0;


        public static async Task Main(string[] args)
        {

            // string url = "https://rezka.ag/animation/page/1/";
            //await program.MainParserForNames(url, httpClientHandler, userAgent);



            // string someurl = Console.ReadLine();
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            //  someurl = "https://rezka.ag/search/?do=search&subaction=search&q=" + someurl;
            //string someurl = "https://rezka.ag/animation/fantasy/56646-reyting-korolya-sunduk-hrabrosti-2023.html";
            //await GoToPageParser(someurl, httpClientHandler, userAgent);
            Program program = new Program();


            await program.chooseWorker();

            stopwatch.Stop();
            Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");


        }

        public async Task chooseWorker() {
            Console.WriteLine("1:for all names\n2:for search");
            Program program = new Program();
            byte choosed = Convert.ToByte(Console.ReadLine());
          
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseCookies = true;
            httpClientHandler.UseDefaultCredentials = false;
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            if (choosed == 1)
            {
                Console.WriteLine("All names Parsing");
                for (; j < 1054; j++)
                {
                    // Console.WriteLine($"https://rezka.ag/page/{j}/");

                    await program.MainParserForNames($"https://rezka.ag/page/{j}/", httpClientHandler, userAgent);
                    //Console.WriteLine(j);
                }

            }
            else if(choosed == 2) {
                Console.WriteLine("Type Searched name");
                string searchName = Console.ReadLine();
                searchName = "https://rezka.ag/search/?do=search&subaction=search&q=" + searchName;
                await program.SearchWorker(searchName, httpClientHandler, userAgent);
            }
            else
            {
                Console.WriteLine("wtf");
                await program.chooseWorker();

            }

        }
        public async Task SearchWorker(string url, HttpClientHandler httpClientHandler, string userAgent)
        {
            var httpClient = new HttpClient(httpClientHandler);

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
                {
                    Console.WriteLine("ID: {0}\n Name: {1}\n Link: {2}\n", item.Item1, item.Item2, item.Item3);
                }
        }


        public async Task MainParserForNames(string url, HttpClientHandler httpClientHandler, string userAgent)
        {
            var httpClient = new HttpClient(httpClientHandler);

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

            // Write the output to a file
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, "output.txt");
            using (var writer = new StreamWriter(filePath, append: true))
            {
                foreach (var item in myList)
                {
                    Console.WriteLine("ID: {0}\n Name: {1}\n Link: {2}\n", item.Item1, item.Item2, item.Item3);

                    writer.WriteLine("ID: {0}\n Name: {1}\n Link: {2}\n", item.Item1, item.Item2, item.Item3);
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