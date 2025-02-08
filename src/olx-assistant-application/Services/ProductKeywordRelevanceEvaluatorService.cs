using olx_assistant_domain.Entities;
using olx_assistant_domain.Entities.Common;

namespace olx_assistant_application.Services;
public static class ProductKeywordRelevanceEvaluatorService
{
    public static float RelevanceValue(Product product, Target target)
    {
        string title = new(product.Title!.Trim().ToLower().Replace(',', ' '));
        string desc = new(product.Description!.Trim().ToLower().Replace(',', ' '));

        var normlized_keywords = target.Keywords?
            .Select(keyword => new Keyword(keyword.Word.ToLower(), keyword.Value))
            .ToList();

        var ratingByTitle = normlized_keywords?
            .Where(keyword => title.Contains(keyword.Word))
            .OrderByDescending(keyword => keyword.Value)
            .FirstOrDefault() ?? new Keyword("null", 0f);

        var ratingByDesc = normlized_keywords?
            .Where(keyword => desc.Contains(keyword.Word))
            .OrderByDescending(keyword => keyword.Value)
            .FirstOrDefault() ?? new("null", 0f);


        return Math.Max((ratingByDesc.Value - ratingByDesc.Value * .25f), ratingByTitle.Value);
    }
}
