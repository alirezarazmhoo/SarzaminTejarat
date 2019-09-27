using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Update
    {
        public int Id { get; set; }
        public int VersionCode { get; set; }
        public string VersionName { get; set; }
        public string Desc { get; set; }
        public string Link { get; set; }
    }
}