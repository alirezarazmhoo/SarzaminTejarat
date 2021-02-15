using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ZarinPal;

namespace WebApplication1.Utility
{
	public static class Purshes
	{
		public const string MerchantID = "5ef84981-58e9-46d7-b5ff-aa15f8b0d74a";
		public const string RedirectUrlFinal = "http://www.sartej.com/Disbursement/Verify/{0}";
		public const string RedirectUrlTest = "http://localhost:8000/Disbursement/Verify/{0}";
		public const string ZarinPalTestUrl = "https://sandbox.zarinpal.com/pg/StartPay/";
		public const string ZarinPalTestFinal = "http://www.zarinpal.com/pg/StartPay/";




		public static PursheResultDto GetResult(long Amount , int Id)
		{
			PursheResultDto pursheResultDto = new PursheResultDto(); 
			ServicePointManager.Expect100Continue = false;
			ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient client = new ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient();
			string authority;
			var Returnurl = string.Format(RedirectUrlFinal, Id);
			int status = client.PaymentRequest(MerchantID, Convert.ToInt32(Amount), "پرداخت سزمین تجارت", string.Empty, string.Empty, Returnurl, out authority);
			pursheResultDto.status = status;
			pursheResultDto.authority = authority; 
			return pursheResultDto;
		}
		public static PursheResultDto Verification(long Amount, int Id,string Authority ,out long refId)
		{
			PursheResultDto pursheResultDto = new PursheResultDto();
			ServicePointManager.Expect100Continue = false;
			ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient client = new ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient();
			int status = client.PaymentVerification(Purshes.MerchantID, Authority , Convert.ToInt32(Amount), out refId);
			pursheResultDto.status = status;
			return pursheResultDto;
		}

		}
	public class PursheResultDto
	{
		public string authority { get; set;  }
		public int status { get; set;  }
	}



}