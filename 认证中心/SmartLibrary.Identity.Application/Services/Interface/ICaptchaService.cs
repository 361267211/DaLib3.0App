using SmartLibrary.Identity.Application.Dtos.Captcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Interface
{
    public interface ICaptchaService
    {
        /// <summary>
        /// 生成随机数字验证码
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        string GenerateRandomNum(int codeLength = 4);
        /// <summary>
        /// 生成随机数字验证码图片
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        CaptchaResultDto GenerateRandomNumImg(int codeLength = 4);
        /// <summary>
        /// 生成随机数字字母验证码
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        string GenerateRandomChapter(int codeLength = 4);
        /// <summary>
        /// 生成随机数字字母验证码图片
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        CaptchaResultDto GenerateRandomChapterImg(int codeLength = 4);
    }
}
