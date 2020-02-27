using Glare.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glare.ViewModels
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public PaginatedList(List<T>pageResults, int count, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            ItemsPerPage = pageSize;
            TotalItems = count;
            TotalPages = (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            this.AddRange(pageResults);
        }
        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
        public static async Task<PaginatedList<T>> ItemAsync(IQueryable<T> itemList, int currentPage, int pageSize)
        {
            var count = await itemList.CountAsync();
            var skip = (currentPage - 1) * pageSize;
            var take = pageSize;
            var pageResults = await itemList.Skip(skip).Take(take).ToListAsync();
            return new PaginatedList<T>(pageResults, count, currentPage, pageSize);
           
           
        }

    }
}
