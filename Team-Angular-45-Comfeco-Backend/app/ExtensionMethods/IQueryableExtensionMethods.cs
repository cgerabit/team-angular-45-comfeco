using BackendComfeco.DTOs.Shared;

using System.Linq;

namespace BackendComfeco.ExtensionMethods
{
    public static class IQueryableExtensionMethods
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Page - 1) * paginacionDTO.RecordsPerPage)
                .Take(paginacionDTO.RecordsPerPage);
        }
    }
}
