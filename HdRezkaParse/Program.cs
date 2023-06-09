﻿using HtmlAgilityPack;
using System.Collections;
using System.Collections.Generic;

namespace MyApp
{
    internal class Program
    {
        HttpClient client = new HttpClient();


        public static int i = 0;
       static HttpClientHandler httpClientHandler = new HttpClientHandler();

        public static async Task Main(string[] args)
        {
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseCookies = true;
            httpClientHandler.UseDefaultCredentials = false;
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;


            //https://rezka.ag/animation/page/2/
            Program program = new Program();

            for (int i = 1; i < 2; i++)
            {
                string url = "https://rezka.ag/animation/page/"+i+"/";
                await program.MainParserForNames(url, httpClientHandler, userAgent);
            }
           
        }

        public async Task MainParserForNames(string url2,HttpClientHandler httpClientHandler,string userAgent)
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




                ////////////  Dictionary ---



                var dict = new Dictionary<int, string>();
                var myList = new List<Tuple<int, string, string>>();

                Program program = new Program();
                if (links != null)
                {
                    foreach (var link in links)
                    {
                        var href = link.GetAttributeValue("href", "");
                        //  Console.WriteLine(href.GetType());
                        // Console.WriteLine(link.InnerText.GetType());

                        myList.Add(new Tuple<int, string, string>(i, link.InnerText, href));
                        i++;
                    }
                }
                foreach (var item in myList)
                {
                    Console.WriteLine("ID: {0}\n Name: {1}\n Link: {2}\n", item.Item1, item.Item2, item.Item3);
                    await GoToPageParser(item.Item3, httpClientHandler, userAgent);
                }

            }

        }




        public static async Task GoToPageParser(string url5, HttpClientHandler httpClientHandler,string userAgent)
        {

            var url = url5;
           

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

                var response = await httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                var html = responseBody;

                var doc2 = new HtmlDocument();
                doc2.LoadHtml(html);

                var diva = doc2.DocumentNode.SelectNodes("//td[@class='l']");


                foreach (var item in diva)
                {
                    Console.WriteLine(item.InnerText);
                }

            }

        }


    }

        
}