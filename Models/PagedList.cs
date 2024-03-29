﻿
namespace ProvaPub.Models
{
    public class PagedList<T>
    {
        public List<T>? Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public bool HasNext => (CurrentPage * PageSize) < TotalCount;

    }
}
