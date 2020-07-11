using SmsIrRestful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Utility
{
	public class SendSms
	{

		Token tk = new Token();
		public string userApiKey { get; } = "785d4086e66492f9924f4083";
		public string secretKey { get; } = "sarzamintrjaratDeveloper";
		public long AdminMobile { get; } = 09136531539;
		public string LineNumber { get; } = "30004747475123";
		List<UltraFastParameters> UltraFastParameters = new List<UltraFastParameters>();
		public string GetToken(string userApiKey , string secretKey)
		{
		string result = tk.GetToken(userApiKey, secretKey);
			return result;
		}
		public int  Send_Sms(string MessageText, string MobileNumber, string token , string lineNumber)
		{
			var messageSendObject = new MessageSendObject()
			{
				Messages = new List<string> { MessageText }.ToArray(),
				MobileNumbers = new List<string> { MobileNumber }.ToArray(),
				LineNumber = lineNumber,
				SendDateTime = null,
				CanContinueInCaseOfError = true
			};
			MessageSendResponseObject messageSendResponseObject = new MessageSend().Send(token, messageSendObject);
			if (messageSendResponseObject.IsSuccessful)
			{
				return 0;
			}
			else
			{
				return 1;
			}
		}

		public void CallSmSMethod(long MobileNumber , int TemplateId , string ParameterName , string ParameterValue)
		{
			var ultraFastSend = new UltraFastSend()
			{
				Mobile = MobileNumber,
				TemplateId = TemplateId,
				ParameterArray = new List<UltraFastParameters>()
				{

				 new UltraFastParameters()
				  {
				   Parameter = ParameterName , ParameterValue = ParameterValue
				  }
				 }.ToArray()
			};
			UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(tk.GetToken(userApiKey, secretKey), ultraFastSend);
		}
		public void CallSmSMethodAdvanced(long MobileNumber, int TemplateId,List<SmsParameters> Parameter)
		{
			foreach (var item in Parameter)
			{
			UltraFastParameters.Add(new UltraFastParameters() { Parameter = item.Parameter, ParameterValue = item.ParameterValue});
			}
			var ultraFastSend = new UltraFastSend()
			{
				Mobile = MobileNumber,
				TemplateId = TemplateId,
				ParameterArray = UltraFastParameters.ToArray()
			};
				UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(tk.GetToken(userApiKey, secretKey), ultraFastSend);
		}
	}
	public class SmsParameters
	{
		public string Parameter { get; set; }
		public string ParameterValue { get; set; }
	}

}