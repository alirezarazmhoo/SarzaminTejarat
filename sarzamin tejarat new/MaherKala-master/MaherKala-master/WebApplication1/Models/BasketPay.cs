using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class BasketPay
	{
		public int Id { get; set; }
		public int MarketerUserId { get; set; }
		public int ProductId { get; set; }
		public int Count { get; set; }


	}
}