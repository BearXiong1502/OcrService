using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrServices.Entites.IvInput.TableType
{
    class USDF_CLSV_PRJ_INFO_OCR
    {
        public string CLSV_PRJ_TYPE { get; set; }
        public string CLSV_PRJ_NAME { get; set; }
        public string CLSV_PRJ_VALUES { get; set; }
        public float IMCO_PRJ_COORDINATE_1_X { get; set; }
        public float IMCO_PRJ_COORDINATE_1_Y { get; set; }
        public float IMCO_PRJ_COORDINATE_2_X { get; set; }
        public float IMCO_PRJ_COORDINATE_2_Y { get; set; }
        public float CONF_1 { get; set; }
        public float CONF_2 { get; set; }
    }
}
