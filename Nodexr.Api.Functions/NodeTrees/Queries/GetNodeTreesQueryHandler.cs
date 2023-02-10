namespace Nodexr.Api.Functions.NodeTrees.Queries;

using Microsoft.EntityFrameworkCore;
using Caesar.Api.Contracts.NodeTrees;
using Caesar.Api.Contracts.Pagination;
using Nodexr.Api.Functions.Common;
using Nodexr.Api.Functions.NodeTrees;

public record GetNodeTreesQuery : IPagedRequest<NodeTreePreviewDto>
{
    public string? SearchString { get; init; }
    public PaginationFilter Pagination { get; init; } = PaginationFilter.All();
}

public record GetNodeTreesQueryHandler(
    INodexrContext nodeTreeContext
) : IPagedRequestHandler<GetNodeTreesQuery, NodeTreePreviewDto>
{
    public async Task<Paged<NodeTreePreviewDto>> Handle(GetNodeTreesQuery request, CancellationToken cancellationToken)
    {
        return await nodeTreeContext.NodeTrees
            .AsNoTracking()
            .WithSearchString(request.SearchString)
            .OrderBy(tree => tree.Title)
            .Select(tree => new NodeTreePreviewDto
            {
                Id = tree.Id,
                Description = tree.Description,
                Expression = tree.Expression,
                Title = tree.Title,
            })
            .ToPagedAsync(
                request.Pagination,
                cancellationToken
            );
    }
}
