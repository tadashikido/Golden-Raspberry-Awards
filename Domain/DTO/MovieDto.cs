using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class MovieDto
    {
        [Name("year")]
        public int Year { get; set; }

        [Name("title")]
        public string Title { get; set; }

        [Name("studios")]
        public string Studio { get; set; }

        [Name("producers")]
        public string Producer { get; set; }

        [Name("winner")]
        public string Winner { get; set; }
    }
}
