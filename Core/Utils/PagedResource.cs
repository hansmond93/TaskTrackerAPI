using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Utils
{
    public class PagedResource<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }

        public bool HasPrevious
        {
            get
            { 
                return CurrentPage > 1; 
            }

        }

        public bool HasNext
        {
            get
            {
                return CurrentPage < TotalPages;
            }

        }

        public PagedResource()
        {
            
        }

        public PagedResource(List<T> items)
        {
            AddRange(items);
        }

        public PagedResource(List<T> items, int count, int pageNumber, int pageSize)
        {
            
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PagedResource<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResource<T>(items, count, pageNumber, pageSize);
        }


    }
}
