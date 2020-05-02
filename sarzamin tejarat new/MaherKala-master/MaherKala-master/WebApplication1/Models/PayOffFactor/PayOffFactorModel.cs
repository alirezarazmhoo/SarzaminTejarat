using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.PayOffFactor
{
	public class PayOffFactorModel
	{
		public string TrackingCode { get; set; }
		public int StatusCode { get; set; }
		public string FactorId { get; set; }
		public string Api_Token { get; set; }

	}
}