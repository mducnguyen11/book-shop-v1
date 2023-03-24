﻿using Bookshop_v5.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace Bookshop_v5.Models.DTO
{
    public class PaginationViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public IDictionary<string, int> RouteValues { get; set; }

        public IEnumerable<Genre> Genres { get; set; }
    }
}
