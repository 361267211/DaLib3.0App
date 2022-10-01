using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DataValidation;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Opac.Shared;
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services.SM.Internal.Attributes;

namespace SmartLibrary.Open.Services.Search
{
    class OpacTemplateService : IOpacTemplateService, IScoped
    {

        private readonly IRepository<OpacTemplate> _opacTemplateRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        public OpacTemplateService(IRepository<OpacTemplate> opacTemplateRepository, IHttpClientFactory httpClientFactory)
        {
            _opacTemplateRepository = opacTemplateRepository;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 检查录入的文件是否合法 即zip文件
        /// </summary>
        /// <param name="link"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private async ValueTask ValidateUploadedFileAsync(string link,string symbol)
        {
            if (string.IsNullOrWhiteSpace(link)) throw new ArgumentException("适配器连接不能为空");
            if (!link.EndsWith(".zip")) throw new NotSupportedException("当前仅支持zip格式的文件");
            using (var httpClient=this._httpClientFactory.CreateClient(nameof(SiteGlobalConfig.FileServer)))
            {
                var tempFilePath = Path.GetTempFileName();
                using (var stream = await httpClient.GetStreamAsync(link))
                {
                    using var fs = File.OpenWrite(tempFilePath);
                    await stream.CopyToAsync(fs);
                }

                var dllPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                ZipFile.ExtractToDirectory(tempFilePath, dllPath);
                foreach (var item in Directory.GetFiles(dllPath, "*.dll", SearchOption.AllDirectories).Where(x => Path.GetFileNameWithoutExtension(x).StartsWith("SmartLibrary.")))
                {
                    var asm = System.Reflection.Assembly.LoadFile(item);
                    foreach (var type in asm.GetTypes())
                    {

                        if (!type.IsAbstract &&
                            type.IsAssignableTo(typeof(IOpacCollectionAdapter))
                            && type.GetCustomAttribute<ConsumeTemplateSymbolAttribute>()?.TemplateSymbol == symbol)
                        {
                            return;
                        }
                    }
                   

                }
            }

            throw new NotSupportedException("传入的文件未实现当前所定义的opac适配器");
        }
        [TransactionScopeEnabled]
        public async Task<Guid> CreateOrModifyOpacTemplateAsync(OpacTemplate model)
        {
            model.Validate();
            var existsItem = await
                this._opacTemplateRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Symbol == model.Symbol, true);
            if (existsItem != null  && existsItem.Id != model.Id)
                throw new ArgumentException("指定标识的模板已存在");
            model.UpdatedTime = DateTimeOffset.Now;
            if (model.DllLink != existsItem?.DllLink)
                await ValidateUploadedFileAsync(model.DllLink,model.Symbol);
            if (existsItem == null)
            {

                existsItem = model;
                existsItem.Id = default;
                existsItem.CreatedTime = model.UpdatedTime.Value;
                existsItem.DeleteFlag = false;
                await this._opacTemplateRepository.InsertAsync(existsItem);
            }
            else
            {
                existsItem.DeleteFlag = false;
                existsItem.Symbol = model.Symbol;
                existsItem.AppointmentSupport = model.AppointmentSupport;
                existsItem.Manufacturer = model.Manufacturer;
                existsItem.Name = model.Name;
                existsItem.Mark = model.Mark;
                existsItem.DllLink = model.DllLink;

            }
            await this._opacTemplateRepository.SaveNowAsync();
            return existsItem.Id;
        }

        public Task<OpacTemplate> FetchOpacTemplateBySymbolAsync(string symbol)
        {
            symbol ??= String.Empty;
            return this._opacTemplateRepository.SingleOrDefaultAsync(x =>
                !x.DeleteFlag && x.Symbol.Trim() == symbol.Trim());


        }

        [CachedMethod(nameof(FetchAllAsync),ExpireOn = 60*30)]
        public Task<IReadOnlyList<OpacTemplate>> FetchAllAsync()
        {
           return  this._opacTemplateRepository.Where(x => !x.DeleteFlag).ToArrayAsync().ContinueWith(x=>x.Result as IReadOnlyList<OpacTemplate>);
        }
    }
}
