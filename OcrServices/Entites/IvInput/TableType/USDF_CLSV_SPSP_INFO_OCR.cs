using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrServices.Entites.IvInput.TableType
{
    public class USDF_CLSV_SPSP_INFO_OCR
    {
        public string CLSV_SPSP_NAME { get; set; }
        public string CLSV_CHG { get; set; }
        public float IMCO_SPSP_COORDINATE_1_X { get; set; }
        public float IMCO_SPSP_COORDINATE_1_Y { get; set; }
        public float IMCO_SPSP_COORDINATE_2_X { get; set; }
        public float IMCO_SPSP_COORDINATE_2_Y { get; set; }
        public float CONF_1 { get; set; }
        public float CONF_2 { get; set; }
    }
}
