﻿namespace Gestao.Client.Libraries.Utilities
{
    public class PaginatedList<T>(List<T> items, int pageIndex, int totalPages)
    {
        public List<T> Items { get; } = items;
        public int PageIndex { get; } = pageIndex;
        public int TotalPages { get; } = totalPages;
        public bool HasNextPage => PageIndex < TotalPages;
        public bool HasPreviousPage => PageIndex > 1;
    }
}
