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
    /// 写入定义环节发送成功的ImimKey
    /// </summary>
    [DatabaseConnection(ConnectionEnum.CustomizeConnectionString, typeof(SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR))]
    public class SPEH_CLIM_CLM_IMIM_STTS_UPDATE_OCR : ICustomizeConnectionString
    {
        public string ConnectionString { get; set; }
        public DataTable pIMIM_STTS_UPDATE { get; set; }

        [SqlParameter(5)] public string pSTS { get; set; }

        [SqlParameter(direction: ParameterDirection.ReturnValue)] public int ReturnValue { get; set; }
    }
}
