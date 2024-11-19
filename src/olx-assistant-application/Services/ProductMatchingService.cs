using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_application.Interfaces.IServices;
using olx_assistant_application.DTOs.Shared;
using olx_assistant_domain.Entities;
using olx_assistant_scraping;
using AutoMapper;

namespace olx_assistant_application.Services;
public class ProductMatchingService : IProductMatchingService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repo;

    public ProductMatchingService(
        IMapper mapper, 
        IProductRepository repository
        ) 
    {
        _mapper = mapper;
        _repo = repository;
    }

    public async Task<List<ProductResponse>> StartMatchingByTargetAsync(Target target)
    {
        Uri paginatedUrl = new Uri($"{target.TargetUri}/?page={1}");

        var scraper = new ProductsScraping(paginatedUrl);
        var products = await scraper.GetProductList();
        var mappedProducts = _mapper.Map<List<Product>, List<ProductResponse>>(products);

        products.ForEach(item => _repo.Create(item));
        _repo.SaveChanges();

        return mappedProducts;
    }
}
