using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcrServices.Entites.Define.TableType;

namespace OcrServices.DataAccess
{

    public interface ICallDataBase
    {
        List<ImageInfo> FindData(DownloadConfig config);
        bool SendJson(List<ImageInfo> imageInfoList, DownloadConfig config);
        bool SendImage(List<ImageInfo> imageInfoList, DownloadConfig config);
        bool GetResult(int imimKey, string json, DownloadConfig config,bool status);
    }
}
