using System;
using System.Collections.Generic;
using System.Data;
using System.Framework.Aop;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrServices.Entites.HpList
{

    /// <summary>
    /// 写入录入环节ocr的json
    /// </summary>
    [DatabaseConnection(ConnectionEnum.CustomizeConnectionString, typeof(SPIN_CLSL_INSERT_OCR))]
    public class SPIN_CLSL_INSERT_OCR : ICustomizeConnectionString
    {
        public string ConnectionString { get; set; }

        /// <summary>
        /// ip+imimKy
        /// </summary>
        public string pFLFL_KY { get; set; }

        /// <summary>
        /// imimKy
        /// </summary>
        [SqlParameter(15)] public string pIMIM_KY { get; set; }

        public DataTable pCLSL_INFO { get; set; }

        [SqlParameter(8000, direction: ParameterDirection.Output)]
        public string pRTN_MSG { get; set; }

        [SqlParameter(direction: ParameterDirection.ReturnValue)]
        public int ReturnValue { get; set; } = 999;
    }
}
