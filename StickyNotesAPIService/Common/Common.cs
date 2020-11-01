using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickyNotesAPIService.Common
{
    public enum ActionStatus
    {
        Successfull = 1,
        Error = 2,
        LoggedOut = 3,
        Unauthorized = 4
    }
    public class ActionOutputBase
    {
        public ActionStatus Status { get; set; }
        public String Message { get; set; }
    }

    public class ActionOutput<T> : ActionOutputBase
    {
        public T Object { get; set; }
        public List<T> List { get; set; }
    }

    public class ActionOutput : ActionOutputBase
    {
        public Int32 ID { get; set; }
    }

    public class PagingResult<T>
    {
        public List<T> List { get; set; }
        public int TotalCount { get; set; }
        public ActionStatus Status { get; set; }
        public String Message { get; set; }
    }
    public class PagingModel
    {
        public int PageNo { get; set; }
        public int RecordsPerPage { get; set; }
        public PagingModel()
        {
            if (PageNo <= 1)
            {
                PageNo = 1;
            }
            if (RecordsPerPage <= 0)
            {
                RecordsPerPage = AppDefaults.PageSize;
            }
        }

        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string Search { get; set; }
        public int UserId { get; set; }
    }
    public static class AppDefaults
    {
        public const Int32 PageSize = 10;
    }
}
