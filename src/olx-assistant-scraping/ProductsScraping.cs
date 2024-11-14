using olx_assistant_domain.Entities.Common;
using System.Text.RegularExpressions;
using olx_assistant_domain.Entities;
using System.Globalization;
using HtmlAgilityPack;


namespace olx_assistant_scraping;
public class ProductsScraping
{
    private readonly HtmlWeb _web = new ();
    private readonly string _domain;
    private readonly Uri _uri;
    public ProductsScraping(Uri uri)
    {
        this._uri = uri;
        this._domain = (Regex.Match(uri.ToString(), @"^(https?://[^/]+)").Groups[1].Value);
    }

    public async Task<List<Product>> GetProductList()
    {
        var htmlDoc = _web.Load(_uri);

        var htmlProductContainer = htmlDoc.DocumentNode.SelectSingleNode("//*[@data-testid=\"listing-grid\"]");
        var htmlProductList = htmlProductContainer.SelectNodes("//*[@data-cy=\"l-card\"]");

        return await GetProductListParallel(htmlProductList);
    }

    private async Task<List<Product>> GetProductListParallel(HtmlNodeCollection htmlList)
    {
        var productTasks = new List<Task<Product>>();

        Parallel.ForEach(htmlList, item =>
        {
            var productUrl = item.SelectSingleNode("//*[@data-cy=\"ad-card-title\"]/a").Attributes["href"].Value;
            var htmlProduct = _web.Load(_domain + productUrl);

            var product = ScrapProductFromHtml(htmlProduct);
            productTasks.Add(product);
        });

        var products = await Task.WhenAll(productTasks);
        return products.ToList();
    }

    private async Task<Product> ScrapProductFromHtml(HtmlDocument html)
    {
        List<Tag> productTags = new List<Tag>();

        var productCreatedDate = html.DocumentNode.SelectSingleNode("//*[@data-testid=\"ad-contact-bar\"]/div/span/span").InnerText;
        var productName = html.DocumentNode.SelectSingleNode("//*[@data-cy=\"ad_title\"]/h4").InnerText;
        var productDesc = html.DocumentNode.SelectSingleNode("//*[@data-cy=\"ad_description\"]/div").InnerText;

        var stringProductPrice = html.DocumentNode.SelectSingleNode("//*[@class=\"css-90xrc0\"]").InnerText;
        var price = decimal.Parse(Regex.Match(stringProductPrice, @"\d+").Value, CultureInfo.InvariantCulture);
        var priceUSD = await CurrencyConverter.ConvertUAH2USD(price);

        var stringProductId = html.DocumentNode.SelectSingleNode("//*[@data-cy=\"ad-footer-bar-section\"]/span").InnerText;
        var productId = int.Parse(Regex.Match(stringProductId, @"\d+").Value, CultureInfo.InvariantCulture);

        var htmlProductTagList = html.DocumentNode.SelectNodes("//*[@class=\"css-rn93um\"]/li");
        foreach (var item in htmlProductTagList)
        {
            var tag = new Tag()
            {
                Name = item.SelectSingleNode("//p/span").InnerHtml,
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
