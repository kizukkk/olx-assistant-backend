﻿using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_application.Interfaces.IServices;
using olx_assistant_domain.Entities;
using olx_assistant_scraping;
using AutoMapper;
using Hangfire;

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

    public void StartMatchingByTarget(Target target)
    {
        Uri paginatedUrl = new Uri($"{target.TargetUri}/?page={1}");

        var scapingJob = 
        BackgroundJob.Enqueue(() => ProcessMatchingJob(paginatedUrl));

        Console.WriteLine($"Started Job with {scapingJob} id");
    }

    public async Task ProcessMatchingJob(Uri url)
    {
        var scraper = new ProductsScraping(url);
        var products = await scraper.GetProductList();

        try
        {
            products.ForEach(item => _repo.Create(item));
            _repo.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }
}
