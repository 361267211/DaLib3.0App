using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services.Search;

namespace SmartLibrary.Open.Application.AppServices
{
    public class OpacAppService : IDynamicApiController
    {
        private readonly IOpacTemplateService _opacTemplateService;

        public OpacAppService(IOpacTemplateService opacTemplateService)
        {
            _opacTemplateService = opacTemplateService;
        }

        [HttpPost]
        public Task<Guid> SaveTemplate([FromBody] OpacTemplate opacTemplate)
        {
            return this._opacTemplateService.CreateOrModifyOpacTemplateAsync(opacTemplate);
        }
    }
}
