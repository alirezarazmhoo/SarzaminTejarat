using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ChatModel
{
	public class PublicChatModel
	{
		public int TotalPage { get; set; }
		public int PageNumber { get; set; }
		public int TotalItemCount { get; set; }
		public List<SendMessage> publicMessage { get; set; }
	}
}