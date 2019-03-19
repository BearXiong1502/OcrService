using System;
using System.Collections.Generic;
using System.Drawing;
using System.Framework.Aop;
using System.Framework.Common;
using System.Framework.Logging;
using System.IO;
using System.Linq;
using System.Text;
using BusinessLogicRepository;
using OcrServices.Entites.Define;
using OcrServices.Entites.Define.TableType;
using OcrServices.Entites.HpList;
using OcrServices.Entites.HpList.TableType;
using OcrServices.Entites.IvInput;
using OcrServices.Entites.IvInput.TableType;

namespace OcrServices.DataAccess
{
    /// <summary>
    /// 影像定义
    /// </summary>
    public class CallDataBaseByDefine : ICallDataBase
    {
        private readonly ICommonBl _commonBl = new CommonBl();

        public List<ImageInfo> FindData(DownloadConfig config)
        {
            var list = _commonBl.QuerySingle<SPEH_CLIM_CLM_IMAGE_FIRST_SEND_LIST_OCR, SPEH_CLIM_CLM_IMAGE_FIRST_SEND_LIST_OCR_RESULT>(new SPEH_CLIM_CLM_IMAGE_FIRST_SEND_LIST_OCR
            {
                ConnectionString = ConnectionStringConfig<SPEH_CLIM_CLM_IMAGE_FIRST_SEND_LIST_OCR>.GetConnectionString(config.ConnectionStringDictionary)
            });
            var guid = Math.Abs(Guid.NewGuid().GetHashCode());
            return list.Select(x => new ImageInfoByDefine(x.IMIM_KY, $"{x.CLNT_BATCH_ID}_{guid}", x.IMIM_PATH_NAME)).Cast<ImageInfo>().ToList();
        }

        public bool SendJson(List<ImageInfo> imageInfoList, DownloadConfig config)
        {
            return true;
        }

        public bool SendImage(List<ImageInfo> imageInfoList, DownloadConfig config)
        {
            var list = imageInfoList.Cast<ImageInfoByDefine>().Select(x => new IMIM_STTS_UPDATE { IMIM_KY = x.ImimKy }).ToList();

            if (list.Count == 0) return false;
            var entity = new SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR
            {
                ConnectionString = ConnectionStringConfig<SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                pIMIM_STTS_UPDATE = list.ToDataTable(),
                pSTS = ((int)ResultJsonStatus.Success).ToString()
            };
            _commonBl.Execute(entity);
            return entity.ReturnValue == 1;
        }

        public bool GetResult(int imimKey, string json, DownloadConfig config, bool status)
        {
            if (!status)
            {
                var e = new SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR
                {
                    ConnectionString = ConnectionStringConfig<SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                    pIMIM_STTS_UPDATE = new List<IMIM_STTS_UPDATE> { new IMIM_STTS_UPDATE { IMIM_KY = imimKey } }.ToDataTable(),
                    pSTS = ((int)ResultJsonStatus.Failed).ToString()
                };
                _commonBl.Execute(e);
                return e.ReturnValue == 1;
            }
            var item = json.FromJson<ImageDefineJson>();
            //item.Type = "0M";
            Console.WriteLine(item.Name + "\t\t" + item.Type);

            var list = new List<IMIM_INFO_INSERT> { new IMIM_INFO_INSERT(imimKey, item.Type, float.Parse(item.Conf) * (float)100) };
            var entity = new SPEH_CLIM_CLM_IMIM_PRE_SECOND_DEFINE_OCR
            {
                ConnectionString = ConnectionStringConfig<SPEH_CLIM_CLM_IMIM_PRE_SECOND_DEFINE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                pIMIM_INFO_INSERT = list.ToDataTable()
            };
            try
            {
                _commonBl.Execute(entity);
                return entity.ReturnValue == 1;
            }
            catch (Exception ex)
            {
                Nlog.Info($"{config.NLogName}.json", $"getresult catch:{ex.Message}");
                var e = new SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR
                {
                    ConnectionString = ConnectionStringConfig<SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                    pIMIM_STTS_UPDATE = new List<IMIM_STTS_UPDATE> { new IMIM_STTS_UPDATE { IMIM_KY = imimKey } }.ToDataTable(),
                    pSTS = ((int)ResultJsonStatus.Failed).ToString()
                };
                _commonBl.Execute(e);
                return false;
            }
        }

    }

    /// <summary>
    /// 发票复核
    /// </summary>
    public class CallDataBaseIvInput : ICallDataBase
    {
        private readonly ICommonBl _commonBl = new CommonBl();
        public List<ImageInfo> FindData(DownloadConfig config)
        {
            var list = _commonBl.QuerySingle<SPEH_CLIM_CLM_IMAGE_SECOND_SEND_LIST_OCR, SPEH_CLIM_CLM_IMAGE_SECOND_SEND_LIST_OCR_RESULT>(new SPEH_CLIM_CLM_IMAGE_SECOND_SEND_LIST_OCR
            {
                ConnectionString = ConnectionStringConfig<SPEH_CLIM_CLM_IMAGE_SECOND_SEND_LIST_OCR>.GetConnectionString(config.ConnectionStringDictionary)
            });
            var guid = Math.Abs(Guid.NewGuid().GetHashCode());
            return list.Select(x => new ImageInfoByIvInput(x.IMIM_KY, $"{x.CLNT_BATCH_ID}_{guid}", x.CLIM_NAME_PATH, x.HPHP_ID, x.SYSV_SOURCE_TYPE)).Cast<ImageInfo>().ToList();
            //return new List<ImageInfo>
            //{
            //    new ImageInfoByIvInput(163446466,"AAA","http://pic2prod-10011668.image.myqcloud.com/d1a90e78-bde6-4e28-a2ef-b9a7a24f5bd1","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446467,"AAA","http://pic2prod-10011668.image.myqcloud.com/0678D730-9784-46DE-BD0D-6ACD79B1E679.jpg","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446468,"AAA","http://pic2prod-10011668.image.myqcloud.com/BE4C7FC6-E9F7-4602-99BC-E8F2226EC7C1.jpg","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446469,"AAA","http://pic2prod-10011668.image.myqcloud.com/f538c8c7-266c-43dc-965f-160b3ffa1554","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446470,"AAA","http://pic2prod-10011668.image.myqcloud.com/c45e960f-a370-4e5d-adc0-798215742450","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446471,"AAA","http://pic2prod-10011668.image.myqcloud.com/39817656-1354-494A-9768-4F44791241C8.jpg","HPID","TYPE")
            //};
        }
        public bool SendJson(List<ImageInfo> imageInfoList, DownloadConfig config)
        {
            var batchNo = imageInfoList.Select(x => x.BatchNo).FirstOrDefault();
            var list = imageInfoList.Cast<ImageInfoByIvInput>().ToList();
            var json = list.Select(x => new { ImKy = $"{config.SourceServerIp}{x.ImimKy}", HpId = x.HphpId, x.SourceType }).ToJson();
            try { File.WriteAllText($@"{config.TargetServerPath}\{config.SourceServerIp}{batchNo}\{config.SourceServerIp}{batchNo}.json", json, Encoding.Default); return true; } catch { return false; }
        }
        public bool SendImage(List<ImageInfo> imageInfoList, DownloadConfig config)
        {
            var batchNo = imageInfoList.Select(x => x.BatchNo).FirstOrDefault();
            var list = imageInfoList.Cast<ImageInfoByIvInput>().ToList();

            var entity = new SPEH_IMAGE_SECOND_SEND_STS_UPDATE_OCR
            {
                ConnectionString = ConnectionStringConfig<SPEH_IMAGE_SECOND_SEND_STS_UPDATE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                pIMIM_KY_LIST = list.Select(x => new Entites.IvInput.TableType.KY_LIST(x.ImimKy)).ToList().ToDataTable(),
                pSTS = ((int)ResultJsonStatus.Success).ToString()
            };
            _commonBl.Execute(entity);
            return entity.ReturnValue == 1;
        }

        public bool GetResult(int imimKey, string json, DownloadConfig config, bool status)
        {
            if (!status)
            {
                var e = new SPEH_IMAGE_SECOND_SEND_STS_UPDATE_OCR
                {
                    ConnectionString = ConnectionStringConfig<SPEH_IMAGE_SECOND_SEND_STS_UPDATE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                    pIMIM_KY_LIST = new List<Entites.IvInput.TableType.KY_LIST> { new Entites.IvInput.TableType.KY_LIST(imimKey) }.ToList().ToDataTable(),
                    pSTS = ((int)ResultJsonStatus.Failed).ToString()
                };
                _commonBl.Execute(e);
                return e.ReturnValue == 1;
            }
            //Console.WriteLine(imimKey + ":" + json);
            //var item = json.FromJson<List<ImageIvInputJson>>();
            var data = json.FromJson<ImageIvInputJson>();
            var item = data.data;
            var prjList = item.Where(x => x.name != "项目2").Select(x => new USDF_CLSV_PRJ_INFO_OCR
            {
                CLSV_PRJ_TYPE = x.name == "项目1" ? "CLIV_PRJ" : "CLIV",
                CLSV_PRJ_NAME = x.name == "项目1" ? x.value1 : x.name,
                CLSV_PRJ_VALUES = x.name == "项目1" ? x.value2 : x.value1,
                IMCO_PRJ_COORDINATE_1_X = !string.IsNullOrEmpty(x.coord1) ? float.Parse(x.coord1.Split(',')[0]) : 0F,
                IMCO_PRJ_COORDINATE_1_Y = !string.IsNullOrEmpty(x.coord1) ? float.Parse(x.coord1.Split(',')[1]) : 0F,
                IMCO_PRJ_COORDINATE_2_X = !string.IsNullOrEmpty(x.coord2) ? float.Parse(x.coord2.Split(',')[0]) : 0F,
                IMCO_PRJ_COORDINATE_2_Y = !string.IsNullOrEmpty(x.coord2) ? float.Parse(x.coord2.Split(',')[1]) : 0F,
                CONF_1 = x.conf1,
                CONF_2 = x.conf2
            }).ToList();

            var spspList = item.Where(x => x.name == "项目2").Select(x => new USDF_CLSV_SPSP_INFO_OCR
            {
                CLSV_SPSP_NAME = x.value1,
                CLSV_CHG = x.value2,
                IMCO_SPSP_COORDINATE_1_X = !string.IsNullOrEmpty(x.coord1) ? float.Parse(x.coord1.Split(',')[0]) : 0F,
                IMCO_SPSP_COORDINATE_1_Y = !string.IsNullOrEmpty(x.coord1) ? float.Parse(x.coord1.Split(',')[1]) : 0F,
                IMCO_SPSP_COORDINATE_2_X = !string.IsNullOrEmpty(x.coord2) ? float.Parse(x.coord2.Split(',')[0]) : 0F,
                IMCO_SPSP_COORDINATE_2_Y = !string.IsNullOrEmpty(x.coord2) ? float.Parse(x.coord2.Split(',')[1]) : 0F,
                CONF_1 = x.conf1,
                CONF_2 = x.conf2
            }).ToList();

            var entity = new SPIN_CLIV_INSERT_OCR
            {
                ConnectionString = ConnectionStringConfig<SPIN_CLIV_INSERT_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                pFLFL_KY = $"{config.SourceServerIp }{ imimKey}",
                pIMIM_KY = imimKey.ToString(),
                pCLIV_ID = item.FirstOrDefault(x => x.name == "NO.")?.value1,
                pCLSV_PRJ_INFO = prjList.ToDataTable(),
                pCLSV_SPSP_INFO = spspList.ToDataTable()
            };
            try
            {
                _commonBl.Execute(entity);
                return entity.ReturnValue == 1;
            }
            catch (Exception ex)
            {
                Nlog.Info($"{config.NLogName}.json", $"getresult catch:{ex.Message}");
                var e = new SPEH_IMAGE_SECOND_SEND_STS_UPDATE_OCR
                {
                    ConnectionString = ConnectionStringConfig<SPEH_IMAGE_SECOND_SEND_STS_UPDATE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                    pIMIM_KY_LIST = new List<Entites.IvInput.TableType.KY_LIST> { new Entites.IvInput.TableType.KY_LIST(imimKey) }.ToList().ToDataTable(),
                    pSTS = ((int)ResultJsonStatus.Failed).ToString()
                };
                _commonBl.Execute(e);
                return false;
            }
        }


    }

    /// <summary>
    /// 住院清单
    /// </summary>
    public class CallDataBaseHospitalList : ICallDataBase
    {
        private readonly ICommonBl _commonBl = new CommonBl();
        public List<ImageInfo> FindData(DownloadConfig config)
        {
            var list = _commonBl.QuerySingle<SPEH_CLID_CLM_IMAGE_DETAIL_SEND_LIST_OCR, SPEH_CLID_CLM_IMAGE_DETAIL_SEND_LIST_OCR_RESULT>(new SPEH_CLID_CLM_IMAGE_DETAIL_SEND_LIST_OCR
            {
                ConnectionString = ConnectionStringConfig<SPEH_CLID_CLM_IMAGE_DETAIL_SEND_LIST_OCR>.GetConnectionString(config.ConnectionStringDictionary)
            });
            var guid = Math.Abs(Guid.NewGuid().GetHashCode());
            return list.Select(x => new ImageInfoByHpList(x.IMIM_KY, $"{x.CLNT_BATCH_ID}_{guid}", x.CLIM_NAME_PATH, x.HPHP_ID, x.SYSV_SOURCE_TYPE,x.IMMI_COORDINATE_INFO)).Cast<ImageInfo>().ToList();
            //return new List<ImageInfo>
            //{
            //    new ImageInfoByIvInput(163446466,"AAA","http://pic2prod-10011668.image.myqcloud.com/d1a90e78-bde6-4e28-a2ef-b9a7a24f5bd1","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446467,"AAA","http://pic2prod-10011668.image.myqcloud.com/0678D730-9784-46DE-BD0D-6ACD79B1E679.jpg","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446468,"AAA","http://pic2prod-10011668.image.myqcloud.com/BE4C7FC6-E9F7-4602-99BC-E8F2226EC7C1.jpg","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446469,"AAA","http://pic2prod-10011668.image.myqcloud.com/f538c8c7-266c-43dc-965f-160b3ffa1554","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446470,"AAA","http://pic2prod-10011668.image.myqcloud.com/c45e960f-a370-4e5d-adc0-798215742450","HPID","TYPE"),
            //    new ImageInfoByIvInput(163446471,"AAA","http://pic2prod-10011668.image.myqcloud.com/39817656-1354-494A-9768-4F44791241C8.jpg","HPID","TYPE")
            //};
        }
        public bool SendJson(List<ImageInfo> imageInfoList, DownloadConfig config)
        {
            var batchNo = imageInfoList.Select(x => x.BatchNo).FirstOrDefault();
            var list = imageInfoList.Cast<ImageInfoByHpList>().ToList();
            var json = list.Select(x => new { ImKy = $"{config.SourceServerIp}{x.ImimKy}", x.SourceType, coords = x.Coords }).ToJson();
            try { File.WriteAllText($@"{config.TargetServerPath}\{config.SourceServerIp}{batchNo}\{config.SourceServerIp}{batchNo}.json", json, Encoding.Default); return true; } catch { return false; }
        }
        public bool SendImage(List<ImageInfo> imageInfoList, DownloadConfig config)
        {
            var batchNo = imageInfoList.Select(x => x.BatchNo).FirstOrDefault();
            var list = imageInfoList.Cast<ImageInfoByHpList>().ToList();

            var entity = new SPEH_IMDI_IMAGE_DETAIL_INFO_SENT_STS_UPDATE_OCR
            {
                ConnectionString = ConnectionStringConfig<SPEH_IMDI_IMAGE_DETAIL_INFO_SENT_STS_UPDATE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                pIMIM_KY_LIST = list.Select(x => new Entites.HpList.TableType.KY_LIST(x.ImimKy)).ToList().ToDataTable(),
                pSTS = ((int)ResultJsonStatus.Success).ToString()
            };
            _commonBl.Execute(entity);
            return entity.ReturnValue == 1;
        }

        public bool GetResult(int imimKey, string json, DownloadConfig config, bool status)
        {
            if (!status)
            {
                var e = new SPEH_IMDI_IMAGE_DETAIL_INFO_SENT_STS_UPDATE_OCR
                {
                    ConnectionString = ConnectionStringConfig<SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                    pIMIM_KY_LIST = new List<Entites.HpList.TableType.KY_LIST> { new Entites.HpList.TableType.KY_LIST(imimKey) }.ToDataTable(),
                    pSTS = ((int)ResultJsonStatus.Failed).ToString(),
                };
                _commonBl.Execute(e);
                return e.ReturnValue == 1;
            }
            var item = json.FromJson<HospitalListJson>();
            //Console.WriteLine(imimKey + ":" + json);

            var pclslList = item.data.Select(x => new USDF_CLSL_INFO_OCR
            {
                CLSL_TYPE = x.name.Contains(@"门诊") ? "1" : x.name.Contains(@"住院") ? "2" : "0",
                CLSL_NAME = x.value1,
                CLSL_VALUES = x.value2,
                CLSL_VALUES_3 = x.value3,
                CLSL_VALUES_4 = x.value4,
                CLSL_VALUES_5 = x.value5,
                IMCO_COORDINATE_INFO = x.coord.ToJson(),
                //IMCO_PRJ_COORDINATE_1_Y = !string.IsNullOrEmpty(x.coord1) ? float.Parse(x.coord1.Split(',')[1]) : 0F,
                //IMCO_PRJ_COORDINATE_2_X = !string.IsNullOrEmpty(x.coord2) ? float.Parse(x.coord2.Split(',')[0]) : 0F,
                //IMCO_PRJ_COORDINATE_2_Y = !string.IsNullOrEmpty(x.coord2) ? float.Parse(x.coord2.Split(',')[1]) : 0F,
                CONF_1 = x.conf1,
                CONF_2 = x.conf2,
                CONF_3 = x.conf3,
                CONF_4 = x.conf4,
                CONF_5 = x.conf5,
            }).ToList();

            var entity = new SPIN_CLSL_INSERT_OCR
            {
                ConnectionString = ConnectionStringConfig<SPIN_CLSL_INSERT_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                pFLFL_KY = $"{config.SourceServerIp }{ imimKey}",
                pIMIM_KY = imimKey.ToString(),
                pCLSL_INFO = pclslList.ToDataTable(),
            };
            try
            {
                _commonBl.Execute(entity);
                return entity.ReturnValue == 1;
            }
            catch (Exception ex)
            {
                Nlog.Info($"{config.NLogName}.json", $"getresult catch:{ex.Message}");
                var e = new SPEH_IMDI_IMAGE_DETAIL_INFO_SENT_STS_UPDATE_OCR
                {
                    ConnectionString = ConnectionStringConfig<SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR>.GetConnectionString(config.ConnectionStringDictionary),
                    pIMIM_KY_LIST = new List<Entites.HpList.TableType.KY_LIST> { new Entites.HpList.TableType.KY_LIST(imimKey) }.ToDataTable(),
                    pSTS = ((int)ResultJsonStatus.Failed).ToString(),
                };
                _commonBl.Execute(e);
                return false;
            }
        }
    }
}

