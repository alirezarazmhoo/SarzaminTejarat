using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
	public class CreditPaymentRequestAttempPictures
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; }
		public int creditPaymentRequestAttempId { get; set; }
		public CreditPaymentRequestAttemp creditPaymentRequestAttemp { get; set; }
	}
}