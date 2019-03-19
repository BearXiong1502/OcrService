using System;
using System.Collections.Generic;
using System.Framework.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcrServices;

namespace OcrServices
{
    public class DownloadConfig
    {
        public string ServiceName { get; set; }
        /// <summary>
        /// 缺位补0,例(127.0.0.1 补为 127000000001(127 000 000 001)
        /// </summary>
        public string SourceServerIp { get; set; }
        public string NLogName { get; set; }
        public string RpcUrl { get; set; }

        public Dictionary<string, string> ConnectionStringDictionary { get; set; }
        /// <summary>
        /// 云平台下载配置,如果不使用,则为空
        /// </summary>
        public CloudConfig CloudConfig { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TargetServerPath { get; set; }

        public string MaxDegreeOfParallelism { get; set; }
        public int SendMillisecondsTimeout { get; set; }
        /// <summary>
        /// 获取数据ServiceName
        /// </summary>
        public string CallDataBaseServiceName { get; set; }

    }

    public class CloudConfig
    {
        public int AppId { get; set; }
        public string SecretId { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
    }

    public abstract class ImageInfo
    {
        public abstract int ImimKy { get; set; }
        public abstract string BatchNo { get; set; }
        public abstract string ImimPath { get; set; }
        public bool IsSuccess { get; set; } = false;
    }

    public class ImageInfoByDefine : ImageInfo
    {
        public ImageInfoByDefine(int imimKy, string batchNo, string imimPath)
        {
            ImimKy = imimKy;
            BatchNo = batchNo;
            ImimPath = imimPath;

        }
        public sealed override int ImimKy { get; set; }
        public sealed override string BatchNo { get; set; }
        public sealed override string ImimPath { get; set; }


    }

    public class ImageInfoByIvInput : ImageInfo
    {
        public ImageInfoByIvInput(int imimKy, string batchNo, string imimPath, string hphpId, string sourceType)
        {
            ImimKy = imimKy;
            BatchNo = batchNo;
            ImimPath = imimPath;
            HphpId = hphpId;
            SourceType = sourceType;
        }
        public sealed override int ImimKy { get; set; }
        public sealed override string BatchNo { get; set; }
        public sealed override string ImimPath { get; set; }
        public string HphpId { get; set; }
        public string SourceType { get; set; }
    }

    public class ImageInfoByHpList : ImageInfo
    {
        private readonly string _jsonData;
        public ImageInfoByHpList(int imimKy, string batchNo, string imimPath, string hphpId, string sourceType, string jsonData)
        {
            ImimKy = imimKy;
            BatchNo = batchNo;
            ImimPath = imimPath;
            HphpId = hphpId;
            SourceType = sourceType;
            _jsonData = jsonData;
        }
        public sealed override int ImimKy { get; set; }
        public sealed override string BatchNo { get; set; }
        public sealed override string ImimPath { get; set; }
        public string HphpId { get; set; }
        public string SourceType { get; set; }

        public HpListCoords[] Coords
        {
            get
            {
                if ((_jsonData ?? "").Length == 0) return new HpListCoords[0];
                var data = _jsonData.FromJson<dynamic>();
                string points = data.Polygon.ToString();
                return points.FromJson<dynamic[]>().Take(4).Select(x => new HpListCoords { X = float.Parse(x["X"].ToString()), Y = float.Parse(x["Y"].ToString()) }).ToArray();
                //return _jsonData.FromJson<HpListCoords[]>();
            }
        }
    }

    public class HpListCoords
    {
        public float X { get; set; }
        public float Y { get; set; }
    }



    public enum ResultJsonStatus
    {
        Extended0,
        Success,
        Extended2,
        Extended3,
        Extended4,
        Extended5,
        Extended6,
        Extended7,
        Extended8,
        Failed
    }

    public class ImageDefineJson
    {
        public string TypeNo { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Conf { get; set; }
    }
    public class ImageIvInputJson
    {
        public List<ImageIvInputList> data { get; set; }
    }
    public class ImageIvInputList
    {
        public string name { get; set; }
        public string value1 { get; set; }
        public string value2 { get; set; }
        public string coord1 { get; set; }
        public string coord2 { get; set; }
        public float conf1 { get; set; }
        public float conf2 { get; set; }
    }
}

public class HospitalListJson
{
    public List<HospitalList> data { get; set; }
}

public class HospitalList
{
    public string name { get; set; }
    public string value1 { get; set; }
    public string value2 { get; set; }
    public string value3 { get; set; }
    public string value4 { get; set; }
    public string value5 { get; set; }
    public HpListCoords[] coord { get; set; }
    public int conf1 { get; set; }
    public float conf2 { get; set; }
    public int conf3 { get; set; }
    public int conf4 { get; set; }
    public int conf5 { get; set; }
}

public class MyRectangleF
{
    public MyRectangleF(string x, string y, string width, string height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
    public string X { get; set; }
    public string Y { get; set; }
    public string Width { get; set; }
    public string Height { get; set; }
}
