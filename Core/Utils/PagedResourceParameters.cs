using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public class PagedResourceParameters
    {
        const int maxPageSize = 50;

        private int _pageSize = 10;


        public int PageNumber { get; set; } = 1;
        public int PageSize 
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string searchQuery { get; set; }
    }
}
