using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Framework.Aop;
using Microsoft.SqlServer.Server;

namespace OcrServices.Entites
{
    [DatabaseConnection(ConnectionEnum.DefaultConnectionString)]
    public class BearTest
    {
        public List<SqlDataRecord> pTKTK_KY { get; set; }

        public string test { get; set; }

        [SqlParameter(20, ParameterDirection.Output)]
        public string outpar { get; set; }
        [SqlParameter(direction: ParameterDirection.ReturnValue)]
        public int ret { get; set; }
    }

    public class IMIM_PATH
    {
        public string IMIM_PATH_NAME { get; set; }
    }
}
