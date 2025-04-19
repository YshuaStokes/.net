using System.Collections.Generic;

namespace api.multitracks.com.Models
{
    public class PaginationRequest
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;
        
        public int PageNumber 
        { 
            get => _pageNumber; 
            set => _pageNumber = value < 1 ? 1 : value; 
        }
        
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = (value < 1 || value > 100) ? _pageSize : value; 
        }
    }

    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (totalCount + pageSize - 1) / pageSize;
        }
    }
}