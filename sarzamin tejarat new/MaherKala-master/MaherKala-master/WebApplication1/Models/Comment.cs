using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Text { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public bool Status { get; set; }
        public int? Parent_id { get; set; }
        public DateTime Date { get; set; }
    }
}