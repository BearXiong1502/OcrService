using System;
using System.Collections.Generic;
using System.Data;
using System.Framework.Aop;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace OcrServices.Entites.Define
{
    /// <summary>
    /// 写入定义环节ocr的json
    /// </summary>
    [DatabaseConnection(ConnectionEnum.CustomizeConnectionString, typeof(SPEH_CLIM_CLM_IMIM_PRE_SECOND_DEFINE_OCR))]
    public class SPEH_CLIM_CLM_IMIM_PRE_SECOND_DEFINE_OCR: ICustomizeConnectionString
    {
        public string ConnectionString { get; set; }
        public DataTable pIMIM_INFO_INSERT { get; set; }

        [SqlParameter(direction: ParameterDirection.ReturnValue)]
        public int ReturnValue { get; set; }
    }
}



//{
//  "ServiceName": "Cloud",
//  "SourceServerIp": "YnDev",
//  "NLogName": ".住院清单",
//  "RpcUrl": "http://192.168.102.20:8888",
//  "ConnectionStringDictionary": {
//    "Default": "Data Source=43.254.47.10,41433;Initial Catalog=TPA_PROD;UID=YNTESTIR;password=YNTESTIR123;Pooling=true;Max Pool Size=32767;Min Pool Size=0;",
//    "SPIN_CLSL_INSERT_OCR": "Data Source=43.254.47.10,41433;Initial Catalog=TPA_INTFS;UID=YNTESTIR;password=YNTESTIR123;Pooling=true;Max Pool Size=32767;Min Pool Size=0;"
//  },
//  "CloudConfig": {
//    "AppId": 10011668,
//    "SecretId": "AKIDgPUCM8JJaUok88LlpTqRruKmA7YZGIMq",
//    "SecretKey": "yFVFSPMhsh3wkE2RO97WPo1RSfOEsXfD",
//    "BucketName": "pic2prod"
//  },
//  "TargetServerPath": "C:\\Test",
//  "MaxDegreeOfParallelism": 8,
//  "SendMillisecondsTimeout": 500000,
//  "CallDataBaseServiceName": "HpList"
//}