﻿namespace MicroBeard.Entities.Parameters
{
    public abstract class QueryStringParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
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

        public string OrderBy { get; set; }

        public bool ExpandRelations { get; set; } = true;

    }
}
