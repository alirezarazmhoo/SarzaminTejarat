using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ChatModel
{
	public class UserChatModel
	{
		public int TotalPage { get; set; }
		public int PageNumber { get; set; }
		public int TotalItemCount { get; set; }
		public List<UserConversions> Message { get; set; }
	}
}