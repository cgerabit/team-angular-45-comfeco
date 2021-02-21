using BackendComfeco.Settings;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.ExtensionMethods
{
    public static class HttpContextExtensionMethods
    {
        public async static Task InserPaginationHeader<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double recordsCount = await queryable.CountAsync();
            httpContext.Response.Headers.Add(ApplicationConstants.CountOfRecordsHeaderName, recordsCount.ToString());
        }

    }
}
