using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

 
namespace WebApplication1
{
    public class PagedItem<T>
    {
        public string LocalUrl { get; set; }
        public int TotalPage { get; set; }
        public int PageNumber { get; set; }
        public int TotalItemCount { get; set; }
        public int Status { get; set; }
        public string Url { get; set; }
        public IPagedList Data { get; set; }
        public PagedItem(IOrderedQueryable<T> query, string url, int size = 8)
        {
            int page = 1;

            if (HttpContext.Current.Request.HttpMethod == "GET")
            {
                if (HttpContext.Current.Request.QueryString["PageNumber"] != null)
                {
                    page = Convert.ToInt32(HttpContext.Current.Request.QueryString["PageNumber"]);
                }
                if (HttpContext.Current.Request.QueryString["PageSize"] != null)
                {
                    page = Convert.ToInt32(HttpContext.Current.Request.QueryString["PageSize"]);
                }
            }
            else
            {
                if (HttpContext.Current.Request.Form.AllKeys.Contains("PageNumber"))
                {
                    page = Convert.ToInt32(HttpContext.Current.Request.Form["PageNumber"]);
                }
                if (HttpContext.Current.Request.Form.AllKeys.Contains("PageSize"))
                {
                    size = Convert.ToInt32(HttpContext.Current.Request.Form["PageSize"]);
                }
            }
            var pagedList = query.ToPagedList(page, size);
            this.TotalPage = pagedList.PageCount;
            this.PageNumber = pagedList.PageNumber;
            this.TotalItemCount = pagedList.TotalItemCount;
            this.Data = pagedList;
            this.Url = url;
            this.Status = 0;

            this.LocalUrl = this.Url.Contains('?') ? this.Url + "&" : this.Url + "?";
        }
        public object GetLinks()
        {
            int current = this.PageNumber,
     last = this.TotalPage,
     delta = 3,
     left = current - delta,
     right = current + delta + 1;
            var range = new List<int>();
            var rangeWithDots = "<nav aria-label='...'><ul class='pagination'>";

            int? l = null;

            for (int i = 1; i <= last; i++)
            {
                if (i == 1 || i == last || i >= left && i < right)
                {
                    range.Add(i);
                }
            }

            foreach (int i in range)
            {
                if (l != null)
                {
                    if (i - l == 2)
                    {
                        if (current == (l + 1))
                            rangeWithDots += "<li class='page-item'><a class='page-link' href='" + this.LocalUrl + "PageNumber=" + (l + 1) + "'>" + (l + 1) + "</a></li>";
                        else
                            rangeWithDots += "<li class='page-item active'><a class='page-link' href='" + this.LocalUrl + "PageNumber=" + (l + 1) + "'>" + (l + 1) + "</a></li>";

                    }
                    else if (i - l != 1)
                    {
                        rangeWithDots += "<li class='page-item'><a class='page-link' href='#'> ... </a></li>";
                    }
                }
                if (current == i)
                    rangeWithDots += "<li class='page-item active'><a class='page-link' href='" + this.LocalUrl + "PageNumber=" + i + "'>" + (i) + "</a></li>";
                else
                    rangeWithDots += "<li class='page-item'><a class='page-link' href='" + this.LocalUrl + "PageNumber=" + i + "'>" + (i) + "</a></li>";

                l = i;
            }
            rangeWithDots += "</ul";
            rangeWithDots += "</nav>";

            return rangeWithDots;

        }
        public object GetAjaxLinks(int cid)
        {
            int current = this.PageNumber,
     last = this.TotalPage,
     delta = 3,
     left = current - delta,
     right = current + delta + 1;
            var range = new List<int>();
            var rangeWithDots = "<nav aria-label='...'><ul class='pagination'>";

            int? l = null;

            for (int i = 1; i <= last; i++)
            {
                if (i == 1 || i == last || i >= left && i < right)
                {
                    range.Add(i);
                }
            }

            foreach (int i in range)
            {
                if (l != null)
                {
                    if (i - l == 2)
                    {
                        if (current == (l + 1))
                            rangeWithDots += "<li class='page-item'><span class='page-link' onclick=CategoryAjax(" + cid+","+(l + 1) + ",0)>" + (l + 1) + "</span></li>";
                        else
                            rangeWithDots += "<li class='page-item active'><span class='page-link' onclick=CategoryAjax(" + cid+","+(l + 1) + ",0)>" + (l + 1) + "</span></li>";

                    }
                    else if (i - l != 1)
                    {
                        rangeWithDots += "<li class='page-item'><a class='page-link' href='#'> ... </a></li>";
                    }
                }
                if (current == i)
                    rangeWithDots += "<li class='page-item active'><span class='page-link' onclick=CategoryAjax(" + cid+"," + i + ",0)>" + (i) + "</span></li>";
                else
                    rangeWithDots += "<li class='page-item'><span class='page-link'onclick=CategoryAjax("+ cid+","+i + ",0)>" + (i) + "</span></li>";

                l = i;
            }
            rangeWithDots += "</ul";
            rangeWithDots += "</nav>";
            return rangeWithDots;

        }

    }
}