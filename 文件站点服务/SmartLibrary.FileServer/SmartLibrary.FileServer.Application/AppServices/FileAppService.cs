/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using SmartLibrary.FileServer.Common.Const;
using SmartLibrary.FileServer.Common.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmartLibrary.FileServer.Application.AppServices
{
    public class FileAppService : IDynamicApiController
    {
        public readonly IWebHostEnvironment _webHostEnvironment;
        public FileAppService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpGet, NonUnify]
        [Authorize]
        public IActionResult FileDownload(string path)
        {
            path = HttpUtility.UrlDecode(path);

            if (!SiteGlobalConfig.FileExtNames.Contains(Path.GetExtension(path).ToLower()))
            {
                throw Oops.Oh($"下载文件失败,仅支持下载包含[{SiteGlobalConfig.FileConfigInfo.FileExtNames}]扩展名的文件").StatusCode(HttpStatusKeys.ExceptionCode);
            }

            string fullPath = _webHostEnvironment.WebRootPath + ($"\\uploads\\{path}"); ;
             var file= new FileStreamResult(new FileStream(fullPath, FileMode.Open), "application/octet-stream") { FileDownloadName = path };
           // var xxx=file.FileStream.
            return file;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<List<string>> UploadFileAsync(List<IFormFile> files)
        {
           
            // 保存到网站根目录下的 uploads 目录
            var savePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            List<string> responseFilePaths = new List<string>();

            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //校验拓展名是否合规
                    if (!SiteGlobalConfig.FileExtNames.Contains(Path.GetExtension(formFile.FileName).ToLower()))
                    {
                        throw Oops.Oh($"上传文件失败,请上传包含[{SiteGlobalConfig.FileConfigInfo.FileExtNames}]扩展名的文件").StatusCode(HttpStatusKeys.ExceptionCode);
                    }

                    //提取租户标示
                    var orgCode = App.User.FindFirst(e => e.Type == "OrgCode").Value;
                    var date = DateTime.Now.ToString("yyyyMMdd");
                    var folder = $"{orgCode}/{date}";


                    // 避免文件名重复，采用 GUID 生成
                    var folderPath = Path.Combine(savePath, folder);  // 可以替代为你需要存储的真实路径

                    //判断文件夹是否存在
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    //加密文件名，生成存储路径
                    var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(formFile.FileName);
                    var filePath = Path.Combine(folderPath, fileName);  // 可以替代为你需要存储的真实路径


                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    responseFilePaths.Add($"/uploads/{orgCode}/{date}/{fileName}");
                }
            }

            // 在动态 API 直接返回对象即可，无需 OK 和 IActionResult
            return responseFilePaths;
        }
    }
}
