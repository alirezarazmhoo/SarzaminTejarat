using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class CheckPaymentRequestAttempPictures
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; }
		public int checkPaymentRequestAttempId { get; set; }
		public CheckPaymentRequestAttemp checkPaymentRequestAttemp { get; set; }
	}
}