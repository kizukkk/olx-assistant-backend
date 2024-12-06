using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_contracts.Interfaces.IRepositories;
using olx_assistant_application.Interfaces.IServices;
using olx_assistant_contracts.Interfaces.IServices;
using olx_assistant_domain.Entities.Common;
using olx_assistant_domain.Entities;
using olx_assistant_scraping;
using Hangfire.Server;
using AutoMapper;
using Hangfire;

namespace olx_assistant_application.Services;
public class ProductMatchingService : IProductMatchingService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repo;
    private readonly IJobTargetRepository _jobTarget;
    private readonly IProductCacheService _cache;

    public ProductMatchingService(
        IMapper mapper, 
        IProductRepository repository,
        IProductCacheService cache,
        IJobTargetRepository jobTarget
        ) 
    {
        _mapper = mapper;
        _repo = repository;
        _cache = cache;
        _jobTarget = jobTarget;
    }

    public void StartMatchingByTarget(Target target)
    {
        Uri paginatedUrl = new Uri($"{target.TargetUri}/?page={1}");

        var scapingJob = 
        BackgroundJob.Enqueue(() => ProcessMatchingJob(paginatedUrl, null));

        RegisterTask(scapingJob, target.Id);
    }

    public async Task ProcessMatchingJob(Uri url, PerformContext context)
    {
        string jobId = context.BackgroundJob.Id;

        var semaphore = new SemaphoreSlim(10);
        var scraper = new ProductsScraper(url);

        var productsId = scraper.GetProductsIdFromPage();

        var nonProcessedTask = productsId.Select(async e =>
        {
            await semaphore.WaitAsync();
            try
            {
                return new
                {
                    Id = e,
                    isCached = await _cache.ProductIsCached(e)
                };
            }
            finally
            {
                semaphore.Release();
            }
        });
        var results = await Task.WhenAll(nonProcessedTask);

        var nonProcessed = results.Where(r => !r.isCached).Select(r => r.Id).ToList();

        var products = await scraper.GetProductListParallelAsync(nonProcessed);

        products.ForEach(item => AddProcessedProduct(jobId, item));
    }

    private void AddProcessedProduct(string jobId, Product product)
    {
        product.ProcessedByTaskId = jobId;
        _repo.Create(product);
        _repo.SaveChanges();
    }

    private void RegisterTask(string jobId, int targetId)
    {
        var targetTask = new TargetJob { jobId = jobId, TargetId = targetId };
        _jobTarget.RegisterTask(targetTask);

    }
}
