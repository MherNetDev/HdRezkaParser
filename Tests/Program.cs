﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyApp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseCookies = true;
            httpClientHandler.UseDefaultCredentials = false;
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            Program program = new Program();
            string someurl = Console.ReadLine();
            stopwatch.Start();
            someurl = "https://rezka.ag/search/?do=search&subaction=search&q=" + someurl;
            await program.MainParserForNames(someurl, httpClientHandler, userAgent);
            int result = 0;
            for (int i = 0; i < 100000000; i++)
            {
                result += i;
            }
            stopwatch.Stop();
            Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

            // print the result to the console
            Console.WriteLine($"Result: {result}");
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

                var links = doc.DocumentNode.SelectNodes("//*[@class='b-content__inline_item-link']//a[@href]");

                var myList = new List<Tuple<int, string, string>>();

                if (links != null)
                {
                    var chunkSize = 10;
                    var chunks = links.ChunkBy(chunkSize);

                    var tasks = new List<Task<List<Tuple<int, string, string>>>>();

                    foreach (var chunk in chunks)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            var list = new List<Tuple<int, string, string>>();

                            foreach (var link in chunk)
                            {
                                var href = link.GetAttributeValue("href", "");
                                var name = link.InnerText.Trim();
                                var id = myList.Count;
                                list.Add(new Tuple<int, string, string>(id, name, href));
                            }

                            return list;
                        }));
                    }

                    await Task.WhenAll(tasks);

                    foreach (var task in tasks)
                    {
                        myList.AddRange(task.Result);
                    }
                }

                foreach (var item in myList)
                {
                    Console.WriteLine("ID: {0}\n Name: {1}\n Link: {2}\n", item.Item1, item.Item2, item.Item3);
                }
            }
        }
    }

    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            while (source.Any())
            {
                yield return source.Take(chunkSize);
                source = source.Skip(chunkSize);
            }
        }
    }
}
