using System;
using System.Collections.Generic;
using System.Data;
using System.Framework.Aop;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrServices.Entites.Define.TableType
{
    public class IMIM_INFO_INSERT
    {
        public IMIM_INFO_INSERT(int imimKy,string imimType,float conf)
        {
            IMIM_KY = imimKy;
            SYSV_IMIM_TYPE = imimType;
            CONFIDENCE = conf;
        }
        public int IMIM_KY { get; set; }

        public string SYSV_IMIM_TYPE { get; set; }

        public float CONFIDENCE { get; set; }
    }
}
