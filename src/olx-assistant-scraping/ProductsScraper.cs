﻿using olx_assistant_domain.Entities.Common;
using System.Text.RegularExpressions;
using olx_assistant_domain.Entities;
using System.Globalization;
using StackExchange.Redis;
using HtmlAgilityPack;


namespace olx_assistant_scraping;
public class ProductsScraper
{
    static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect($"localhost:6379");
    static readonly IDatabase db = _redis.GetDatabase();

    private readonly HtmlWeb _web = new ();
    private readonly string _domain;
    private Uri _uri;
    public ProductsScraper(Uri uri)
    {
        this._uri = uri;
        this._domain = (Regex.Match(uri.ToString(), @"^(https?://[^/]+)").Groups[1].Value);
    }


    public List<int> GetProductsIdFromPage()
    {
        var htmlDoc = _web.Load(_uri);

        var htmlProductList = htmlDoc.DocumentNode
            .SelectSingleNode("//*[@data-testid=\"listing-grid\"]")
            .SelectNodes("//*[@data-cy=\"l-card\"]");
        
        return htmlProductList.Select(e => int.Parse(e.Attributes["id"].Value)).ToList();
    }

    public async Task<List<Product>> GetProductListParallelAsync(List<int> products)
    {
        var productTasks = new List<Task<Product>>();

        Parallel.ForEach(products, id =>
        {
            var productUrl = new Uri($"{_domain}/{id}");
            var htmlProduct = _web.Load(productUrl);

            var product = ScrapProductFromHtml(htmlProduct);
            productTasks.Add(product);
        });

        var result = await Task.WhenAll(productTasks);
        return result.ToList();
    }

    private async Task<Product> ScrapProductFromHtml(HtmlDocument html)
    {
        List<Tag> productTags = new List<Tag>();

        var productCreatedDate = html.DocumentNode.SelectSingleNode("//*[@data-testid=\"ad-contact-bar\"]/div/span/span").InnerText;
        var productName = html.DocumentNode.SelectSingleNode("//*[@data-cy=\"ad_title\"]/h4").InnerText;
        var productDesc = html.DocumentNode.SelectSingleNode("//*[@data-cy=\"ad_description\"]/div").InnerText;

        var stringProductPrice = html.DocumentNode.SelectSingleNode("//*[@class=\"css-90xrc0\"]").InnerText.Replace(" ", String.Empty);
        var price = decimal.Parse(Regex.Match(stringProductPrice, @"\d+").Value, CultureInfo.InvariantCulture);
        var priceUSD = stringProductPrice.Contains("грн") ? await CurrencyConverter.ConvertUAH2USD(price) : price;

        var stringProductId = html.DocumentNode.SelectSingleNode("//*[@data-cy=\"ad-footer-bar-section\"]/span").InnerText;
        var productId = int.Parse(Regex.Match(stringProductId, @"\d+").Value, CultureInfo.InvariantCulture);

        var htmlProductTagList = html.DocumentNode.SelectNodes("//*[@class=\"css-rn93um\"]/li");
        foreach (var item in htmlProductTagList)
        {
            var tag = new Tag()
            {
                Name = item.SelectSingleNode("./p").InnerText,
            };
            productTags.Add(tag);
        }

        return new Product()
        {
            ProductId = productId,
            Title = productName,
            Description = productDesc,
            Price = priceUSD,
            Tags = productTags
        };
    }
}
