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

		public string LineNumber { get; } = "30004747475123";

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

		public string CreateUserActiveMessage(string UserName ,string UserLastName , string UserMobile , string UserPassword)
		{		
			string Text1= "حساب کاربری شما فعال گردیده است";
			string Text2 = "نام کاربری شما : " + UserMobile;
			string Text3 = " و رمز عبور شما :" + UserPassword;
			string Text4 = "کاربر گرامی :" + UserName + " " + UserLastName + " " + Text1 + " " + Text2 +" " + Text3 +" می باشد";
			return Text4;
		}
		public string CreateUserRejectByParentMessage(string UserName, string UserLastName)
		{
			string Text1 = "شما توسط معرف خود در سامانه سرزمین تجارت پذیرفته نشدید";
			string Text2 = "کاربر گرامی :" + UserName + " " + UserLastName + " " + Text1 + " ";
			return Text2;
		}
		public string CreateUserPursheForAddSubsetMessage(string UserName, string UserLastName,string Count )
		{
			string Text1 = " خرید تیکت افزایش زیر مجموعه باموفقیت انجام شد";
			string Text2 = "ارزش تیکت : " + Count;
			string Text3 = "کاربر گرامی :" + UserName + " " + UserLastName + " " + Text1 + " "+Text2 +" است ";
			return Text3;
		}
		public string CreateUserPursheForPlanMessage(string UserName, string UserLastName)
		{
			string Text1 = " خرید پلن باموفقیت انجام شد";
			string Text2 = "کاربر گرامی :" + UserName + " " + UserLastName + " " + Text1 + "";
			return Text2;
		}
	}
}