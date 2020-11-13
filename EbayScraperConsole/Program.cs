using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EbayScraperConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var url = "https://www.ebay.com.au/sch/i.html?_from=R40&_nkw=ps5&_in_kw=1&_ex_kw=&_sacat=0&LH_Complete=1&_udlo=&_udhi=&_samilow=&_samihi=&_sadis=15&_stpos=2163&_sargn=-1%26saslc%3D1&_salic=15&_sop=12&_dmd=1&_ipg=200&_fosrp=1";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var productsHtml = htmlDocument.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("ListViewInner")).ToList();

            var productListItems = productsHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("id", "")
                .Contains("item")).ToList();

            Console.WriteLine(productListItems.Count());
            Console.WriteLine();
            foreach (var item in productListItems)
            {
                // id
                Console.WriteLine(item.GetAttributeValue("listingid", ""));

                // productName
                Console.WriteLine(item.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));

                // price
                Console.WriteLine(
                    Regex.Match(
                    item.Descendants("li")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("lvprice prc")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    , @"\d+.\d+"));

                // listing
                Console.WriteLine(
                    item.Descendants("li")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("lvformat")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    );

                // url
                Console.WriteLine(item.Descendants("a").FirstOrDefault().GetAttributeValue("href", ""));

                

                Console.WriteLine();
            }

            Console.ReadLine();




        }

    }
}
