using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Add
using System.ComponentModel.DataAnnotations;

namespace OnlineNewspaper.Models
{
    public class NewsDetails
    {
        [Key]
        public int NewsDetailsID { get; set; }
        public string Title { get; set; }
        public string NewsBody { get; set; }
        public string Image { get; set; }

        public int NewsCategoryID { get; set; }
        public virtual NewsCategory NewsCategory { get; set; }
    }
}
