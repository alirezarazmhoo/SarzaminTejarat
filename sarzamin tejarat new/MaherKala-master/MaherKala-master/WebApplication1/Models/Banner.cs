﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Link { get; set; }
        [MaxLength(1000)]
        public string Image{ get; set; }
        public int Type { get; set; }
    }
}