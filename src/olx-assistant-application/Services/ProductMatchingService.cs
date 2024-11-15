﻿using olx_assistant_application.DTOs.Shared;
using olx_assistant_application.Interfaces;
using olx_assistant_domain.Entities;
using olx_assistant_scraping;
using AutoMapper;

namespace olx_assistant_application.Services;
public class ProductMatchingService : IProductMatchingService
{
    private readonly IMapper _mapper;

    public ProductMatchingService(IMapper mapper) => _mapper = mapper;

    public async Task<List<ProductResponse>> StartMatchingByTargetAsync(Target target)
    {
        Uri paginatedUrl = new Uri($"{target.TargetUri}/?page={1}");

        var scraper = new ProductsScraping(paginatedUrl);
        var products = await scraper.GetProductList();
        var mappedProducts = _mapper.Map<List<Product>, List<ProductResponse>>(products);

        return mappedProducts;
    }
}
