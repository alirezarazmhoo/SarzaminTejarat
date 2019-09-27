using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerChat
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(300,ErrorMessage ="پیام بیش از حد بزرگ است")]
        public string Text { get; set; }
        [Index]
        public long Timestamp { get; set; }
        public MarketerUser User { get; set; }
        public MarketerChat()
        {
            
            Timestamp = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssffff"));
        }

    }
}