using System;
using System.Collections.Generic;
using System.Data;
using System.Framework.Aop;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrServices.Entites.IvInput
{
    /// <summary>
    /// 写入录入环节发送成功的ImimKey
    /// </summary>
    [DatabaseConnection(ConnectionEnum.CustomizeConnectionString, typeof(SPEH_IMAGE_SECOND_SEND_STS_UPDATE_OCR))]
    public class SPEH_IMAGE_SECOND_SEND_STS_UPDATE_OCR : ICustomizeConnectionString
    {
        public string ConnectionString { get; set; }
        public DataTable pIMIM_KY_LIST { get; set; }

        [SqlParameter(5)] public string pSTS { get; set; }

        [SqlParameter(direction: ParameterDirection.ReturnValue)] public int ReturnValue { get; set; }
    }
}
