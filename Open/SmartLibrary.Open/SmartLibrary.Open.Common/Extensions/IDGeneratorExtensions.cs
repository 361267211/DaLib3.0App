using Furion.DistributedIDGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Common.Extensions
{
    public static class IDGeneratorExtensions
    {
        public static Guid CreateGuid(this IDistributedIDGenerator idGenerator, Guid? fixedGuid = null)
        {
            if (fixedGuid == null || fixedGuid == Guid.Empty)
            {
                return new Guid(idGenerator.Create().ToString());
            }
            return fixedGuid.Value;
        }
    }
}
