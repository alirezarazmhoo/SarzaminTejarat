using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ChatModel
{
	public class ChatModel
	{
		public int Id { get; set; }
		public string LastMessage { get; set; }
		public DateTime CreateDate { get; set; }
		public int State { get; set; }
		//public int UserId { get; set; }


	}
}