using SmartLibrary.AppCenter.Application.Dtos.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Common.Dtos
{
    /// <summary>
    /// 模拟数据
    /// </summary>
    public static class MockData
    {
        /// <summary>
        /// 应用类型
        /// </summary>
        /// <returns></returns>
        public static List<DictionaryDto> GetAppType()
        {
            var result = new List<DictionaryDto>();

            result.Add(new DictionaryDto { Id = "30f04f62-ff1e-45be-be16-2cc29020660d", Name = "基础应用", Value = "30f04f62-ff1e-45be-be16-2cc29020660d" });
            result.Add(new DictionaryDto { Id = "ef85e342-f040-4f38-9345-82e4031c0f99", Name = "资源服务", Value = "ef85e342-f040-4f38-9345-82e4031c0f99" });
            result.Add(new DictionaryDto { Id = "ce0a4078-be66-4395-8964-53de6d59fda4", Name = "学术与情报", Value = "ce0a4078-be66-4395-8964-53de6d59fda4" });
            result.Add(new DictionaryDto { Id = "16e14d22-1b5f-4ab2-a9b7-0b361148924a", Name = "阅读推广", Value = "16e14d22-1b5f-4ab2-a9b7-0b361148924a" });
            result.Add(new DictionaryDto { Id = "f2de34bd-04d5-434c-ba5a-0d837dd0bf8b", Name = "空间服务", Value = "f2de34bd-04d5-434c-ba5a-0d837dd0bf8b" });
            result.Add(new DictionaryDto { Id = "2c2a0369-97de-4976-bc89-47bd9b7d12e1", Name = "分析决策", Value = "2c2a0369-97de-4976-bc89-47bd9b7d12e1" });

            return result;
        }

        /// <summary>
        /// 采购类型
        /// </summary>
        /// <returns></returns>
        public static List<DictionaryDto> GetPurchaseType()
        {
            var result = new List<DictionaryDto>();

            result.Add(new DictionaryDto { Id = "f33b3903-1ac0-4d1d-bbc3-fdbc0f08ed09", Name = "正式", Value = "f33b3903-1ac0-4d1d-bbc3-fdbc0f08ed09" });
            result.Add(new DictionaryDto { Id = "681541b0-569d-4079-928e-2acc6dde6933", Name = "试用", Value = "681541b0-569d-4079-928e-2acc6dde6933" });

            return result;
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        /// <returns></returns>
        public static List<DictionaryDto> GetUserTypeList()
        {
            var result = new List<DictionaryDto>();

            result.Add(new DictionaryDto { Id = "46019bc1-e85b-4c9d-9336-822881e6737f", Name = "毕业生", Value = "46019bc1-e85b-4c9d-9336-822881e6737f" });
            result.Add(new DictionaryDto { Id = "3fb65345-6cbe-457a-9574-d34c13835b44", Name = "本科生", Value = "3fb65345-6cbe-457a-9574-d34c13835b44" });
            result.Add(new DictionaryDto { Id = "af9ae502-5655-418a-b180-f61d5f4c1fa5", Name = "校友读者", Value = "af9ae502-5655-418a-b180-f61d5f4c1fa5" });
            result.Add(new DictionaryDto { Id = "36604472-da88-4a96-9968-a36b9dea9f24", Name = "社会读者", Value = "36604472-da88-4a96-9968-a36b9dea9f24" });

            return result;
        }

        /// <summary>
        /// 用户分组
        /// </summary>
        /// <returns></returns>
        public static List<DictionaryDto> GetUserGroupList()
        {
            var result = new List<DictionaryDto>();

            result.Add(new DictionaryDto { Id = "191a4e67-ff56-4dcb-9d09-83d1d46259d7", Name = "2018级读者", Value = "191a4e67-ff56-4dcb-9d09-83d1d46259d7" });
            result.Add(new DictionaryDto { Id = "cec1d913-f984-454c-a59c-d7f96a32d279", Name = "中年读者", Value = "cec1d913-f984-454c-a59c-d7f96a32d279" });
            result.Add(new DictionaryDto { Id = "cf055a6e-181b-4f7e-aa00-9a5f837744e1", Name = "校友读者A", Value = "cf055a6e-181b-4f7e-aa00-9a5f837744e1" });

            return result;
        }

        public static List<string> AppGuids = new List<string>
        {
            "fbafd470-7133-4261-8cf6-10e506306d0b",
            "2de84ea6-5f37-4861-b2b7-a594412eff61",
            "cd8d0b4a-2140-4bb6-b519-72c12cea2541",
            "28bf9072-c015-4543-b343-dcb401463c4b",
            "40588178-58d8-4e8f-8e99-3d5864e0a967",
            "3637fa91-3cc6-41f6-9e2d-53af81c7785f",
            "2f5de5ba-2c2a-4d2e-b646-727deadc2a79",
            "e43fbf52-eb1b-4bc7-b915-d3804641ead1",
            "f5a5bc59-ee55-4133-9eb3-ab24fd16770c",
            "acd8ab99-aab6-497e-854c-e0812a651d08",
            "8e77cea8-5579-48d9-b8b0-6e749f286e80",
            "1bf12a38-8455-4432-81cb-61e3fa2af78b",
            "25916492-b843-4c0b-a960-63fcbedd6b47",
            "ece24b8a-937e-4f32-8443-8ad362ba49a3",
            "f99cbddb-a412-40a0-a735-71b4ee0ad453",
            "d88f77fb-b3f4-402d-92ff-3017cb3e2ab2",
            "68b0d99f-a87f-43cf-9216-abbc80d8b2f6",
            "4a32056e-69ad-4197-85fe-1aff40f585f0",
            "edf9ba6c-3e34-469f-b88c-a515556c524a",
            "7222fce3-9963-4a8c-a410-25d50ad376a6",
            "89cf9584-a8aa-42a8-81de-1d9607f2b2d0",
            "ee160993-7038-4f5e-bbdd-8a3e4b3823aa",
            "be45b3b3-31af-4f07-aae8-9b5d08d3fce9",
            "cda1ec29-8ac4-4a80-bbdb-01e72034fc4c",
            "b4a6ec81-a56f-421d-9072-7dd45bf9e751",
            "6abf1f04-e338-4562-aa0d-5f363fae7d35",
            "525fcdae-2781-407e-9ee9-21e2bd76ca53",
            "23a90097-7325-4f67-8c57-c06d63e0e342",
            "84bf659d-47bd-4a2e-b464-a4aea20fcbba",
            "49fa77b2-1a38-4639-ae08-0b23a2eb5541",
            "93acfa2b-9b62-4439-8e5c-1e2a05e5a2ff",
            "4eb16f36-1f3f-481e-908c-de5639378491",
            "c12e38bd-3205-4f2c-b292-e1e31dc0f9a5",
            "92938803-19bc-43c5-91d8-d306f13920a8",
            "9d418228-db88-4ad4-aca7-0bc53c211629",
            "93e9f625-dafc-4910-b5f1-e5d7679d8d56",
            "2a4fa40a-14f3-4041-a282-b9e1c6106f38",
            "8a31295d-aba1-49ed-9281-2db1861b8875",
            "0218ab52-a1e3-4caa-9950-eade55e8325f",
            "9ad285c1-8f7c-4221-b1a9-6bbc8b26195a",
            "ff3ce83d-4397-47ba-8f06-85126facec0a",
            "6546419e-9f0b-4ecb-9909-4ec5d400d5ff",
            "0c6cbbea-7ccd-4055-95fa-362761f6a315",
            "3cb605ad-5839-4745-8c77-21c072300cd4",
            "a0c24808-fb9d-49fc-9a06-3068a74f5f5a",
            "2027fe05-5cd3-481f-953e-1f90e1bba687",
            "b02b483c-a3ed-4276-b9ad-a7f49283dc1c",
            "5f8568e5-4c68-447e-8405-07692b1a63f3",
            "99e7199b-e5ac-4b17-a2db-ddb636cd9f2c",
            "97a8f787-5027-4855-a53c-7072d8e90a3a",
            "d35e4ea1-6eab-491e-9364-c35118bb38b5",
            "7a48845e-9241-44ac-bed4-8336a7e0e07e",
            "d786d7ae-2c47-476b-a9b9-cdbf960740ce",
            "e0e532ea-71f1-4322-b0ab-7dd2f9665010",
            "42002c04-8745-477a-a742-21ed95ff9f35",
            "a7502137-32c5-4767-b83f-6b0bfc7a237f",
            "b6604893-396e-49c8-aa57-8b1b76d59e3b",
            "ab8da50c-a1ef-4da1-9d4f-34aa434450bd",
            "2e0971b3-5bd1-42d4-93a9-0b9442adde7b",
            "5d4cea3e-7494-41e0-bb17-1aaaeda84dbe",
            "fbd36203-99c3-4edf-90ae-4c7c765f256c",
            "44ab1f00-e58e-4fbb-8c50-347d91ef78ab",
            "8e46abbf-d152-4912-a125-08eb998d49bb",
            "b5e658c4-2f45-446a-ae80-60d868bd5ad7",
            "d30bc4b6-e966-4e53-9445-b9753a1608b4",
            "b1581c9a-61f7-4188-9e6f-2b4f92a2607a",
            "e5b8c5d3-ade7-48e8-9cfd-028b0a40104d",
            "6bba4ec9-da75-4da4-a272-8778351737b3",
            "cd8e86e3-e6ad-40e2-b567-a8a53e7f126c",
            "35dc2650-ad2d-4edf-ae2d-d2b443dbb2b5",
            "fac4ec2b-9f8e-481e-b494-40500cf5c941",
            "50e967b6-3dbe-4825-9aab-e3089cd29653",
            "ed169c55-2f74-409c-82d0-0805f8007c4c",
            "ecda1ce4-b47f-4b3c-b2f0-1821a2b26b57",
            "74913b80-9a34-40d5-8429-35544499013c",
            "ee317d32-7cee-495f-8630-040f274f05a7",
            "29a9a727-d863-49e9-8435-a30286d3b74d",
            "35ed0c21-c094-4af0-9bb9-13ae3226124e",
            "11af0f71-e7fb-4af5-97e4-72a4fd5f615f",
            "8786a7f9-9249-46e4-bbc7-c80930f43d5e",
            "150c0341-385f-4bac-8c5c-948136982a86",
            "5df14fa7-6bca-4efb-aade-a89b135ebc4b",
            "a5a864e5-4920-4224-bc9c-ba1bdd9e56a0",
            "1a2f122b-ca83-4681-b94c-ee70260df058",
            "36f1c9ad-bd03-4b57-ae86-15a24fc528de",
            "cb65a8ce-9dbb-47e0-a5c9-a1c57eeaf26f",
            "807cabdf-a576-4d05-96e4-9a1169274f4e",
            "bf4d1caa-6074-4c59-849f-d22f50464762",
            "0e71c839-7f93-4692-a1a9-55bd148f1f94",
            "bad41a66-5146-4ecf-85d5-918b26cdd987",
            "557d4534-ecf0-4913-bbb6-7b3054841d74",
            "ee83b421-5fbb-4f4b-b40d-8a5c382ede46",
            "8e22eef2-c23e-422b-8d02-e1a475e47a96",
            "e4a9bbcb-ebbd-4645-b7c1-79046430f6f4",
            "eaaae902-aead-4d59-a7b8-15fa2e40a7d4",
            "4018cae6-7af3-4f88-955a-df60636163ad",
            "d10ad6d5-259e-41e6-8f93-492cb3086663",
            "62b029e9-03c7-4bcd-b1ef-8c4d3567a615",
            "73339fb3-fe84-49b6-b556-26f63537e5dc",
            "6ea9fa03-bab0-4bb5-b8af-ded1bf1e6cfa",
        };

        /// <summary>
        /// 所有应用
        /// </summary>
        public static List<AppListDto> GetAllApp()
        {
            var result = new List<AppListDto>();

            for (int i = 0; i < 50; i++)
            {
                result.Add(new AppListDto
                {
                    AppId = AppGuids[i],
                    AppName = $"应用{i + 1}",
                    AppIcon = $"http://192.168.21.48:8001/ContentDelivery/icons/icon({new Random().Next(1, 10)}).png",
                    AppType = GetAppType()[new Random().Next(1, 6)].Id,
                    BackendUrl = "",
                    FrontUrl = "http://192.168.21.71:7002",
                    BeginDate = "2021-11-08",
                    ExpireDate = "2021-12-26",
                    Content = $"应用介绍应用介绍{i + 1}",
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                    CurrentVersion = "V1.0.0",
                    Developer = "维普智图",
                    Price = "35.66",
                    PurchaseType = GetPurchaseType()[new Random().Next(1, 2)].Id,
                    SceneType = "门户",
                    Status = "1",
                    ShowStatus = "启用",
                    Terminal = "PC端",
                    UpdateTime = DateTime.Now.ToString("yyyy-MM-dd")
                });
            }

            return result;
        }

        /// <summary>
        /// 应用更新日志
        /// </summary>
        /// <returns></returns>
        public static List<AppLogDto> GetAppLogs()
        {
            return new List<AppLogDto>
            {
                new AppLogDto { Id = "1", Title = "统一文献检索更新到V2", ReleaseTime = "2021-10-26", Content = "统一文献检索更新到V2" },
                new AppLogDto { Id = "2", Title = "统一文献检索更新到V2", ReleaseTime = "2021-10-26", Content = "统一文献检索更新到V2" }
            };

        }

        /// <summary>
        /// 日志详情
        /// </summary>
        /// <returns></returns>
        public static AppLogDetailDto GetAppLogDetail()
        {
            return new AppLogDetailDto
            {
                Id = "1",
                Title = "统一文献检索更新到V2",
                Content = "统一文献检索更新到V2,统一文献检索更新到V2,统一文献检索更新到V2",
                AppTilte = "统一检索",
                UpdateTime = "2021-10-26",
                Version = "v1.2.0",
                AppIcon = "http://192.168.21.48:8001/ContentDelivery/icons/icon(11).png"
            };
        }
    }
}
