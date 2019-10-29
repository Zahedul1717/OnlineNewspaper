using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineNewspaper.Models
{
    public class NewsCategory
    {
        public int NewsCategoryID { get; set; }
        public string Name { get; set; }

        public ICollection<NewsDetails> NewsDetails { get; set; }
    }
}
