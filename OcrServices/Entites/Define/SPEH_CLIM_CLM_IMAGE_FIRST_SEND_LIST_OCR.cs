using System;
using System.Framework.Aop;

namespace OcrServices.Entites.Define
{
    /// <summary>
    /// 获取定义环节发送的图片
    /// </summary>
    [DatabaseConnection(ConnectionEnum.CustomizeConnectionString, typeof(SPEH_CLIM_CLM_IMAGE_FIRST_SEND_LIST_OCR))]
    public class SPEH_CLIM_CLM_IMAGE_FIRST_SEND_LIST_OCR : ICustomizeConnectionString
    {
        public string ConnectionString { get; set; }
    }

    public class SPEH_CLIM_CLM_IMAGE_FIRST_SEND_LIST_OCR_RESULT
    {
        public int IMIM_KY { get; set; }
        public string CLNT_BATCH_ID { get; set; }
        public string IMIM_PATH_NAME { get; set; }
    }
}
