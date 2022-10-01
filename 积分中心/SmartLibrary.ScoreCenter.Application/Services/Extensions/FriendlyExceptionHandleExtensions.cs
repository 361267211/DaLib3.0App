using Furion.FriendlyException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Extensions
{
    public static class FriendlyExceptionHandleExtensions
    {
        public static AppFriendlyException BadRequest(this AppFriendlyException exception)
        {
            return exception.StatusCode(Consts.Consts.ExceptionStatus);
        }
    }
}
