using MediatR;
using Sticky.Application.Dashboard.Controllers.Category.Dto;
using Sticky.Domain.CategoryAggrigate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Controllers.Category.Handlers
{
    public class SearchCategoryHandler : IRequestHandler<CategorySearchRequest, IEnumerable<CategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        public SearchCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<CategoryResponse>> Handle(CategorySearchRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryRepository.SearchAsync(request.HostId,request.Keyword);
            return result.Select(c => new CategoryResponse
            {
                Counter = c.Counter,
                Category = c.Name,
                Id  =c.Id
            });
        }
    }
}
