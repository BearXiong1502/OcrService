using System;
using System.Collections.Generic;
using System.Data;
using System.Framework.Aop;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrServices.Entites.HpList
{
    [DatabaseConnection(ConnectionEnum.CustomizeConnectionString, typeof(SPEH_IMDI_IMAGE_DETAIL_INFO_SENT_STS_UPDATE_OCR))]
    public class SPEH_IMDI_IMAGE_DETAIL_INFO_SENT_STS_UPDATE_OCR : ICustomizeConnectionString
    {
        public string ConnectionString { get; set; }
        public DataTable pIMIM_KY_LIST { get; set; }

        [SqlParameter(5)] public string pSTS { get; set; }

        [SqlParameter(8000, ParameterDirection.Output)] public string pRTN_MSG { get; set; }

        [SqlParameter(direction: ParameterDirection.ReturnValue)] public int ReturnValue { get; set; } = 999;
    }
}
