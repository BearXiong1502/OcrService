using System;
using System.Collections.Generic;
using System.Framework.Aop;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrServices.Entites.IvInput
{
    /// <summary>
    /// 获取录入环节发送的图片
    /// </summary>
    [DatabaseConnection(ConnectionEnum.CustomizeConnectionString, typeof(SPEH_CLIM_CLM_IMAGE_SECOND_SEND_LIST_OCR))]
    public class SPEH_CLIM_CLM_IMAGE_SECOND_SEND_LIST_OCR : ICustomizeConnectionString
    {
        public string ConnectionString { get; set; }
    }

    public class SPEH_CLIM_CLM_IMAGE_SECOND_SEND_LIST_OCR_RESULT
    {
        public int IMIM_KY { get; set; }
        public string CLNT_BATCH_ID { get; set; }
        public string CLIM_NAME_PATH { get; set; }
        public string HPHP_ID { get; set; }

        public string SYSV_SOURCE_TYPE { get; set; }
    }
}
