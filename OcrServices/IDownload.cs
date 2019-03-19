using System;
using System.Collections.Generic;
using System.Framework.Logging;
using System.IO;
using CookComputing.XmlRpc;
using IOFile;

namespace OcrServices
{
    public interface IDownload
    {
        string NLogName { get; set; }
        bool Download(string sourcePath, string targetPath);
    }

    public class DownloadByFile : IDownload
    {
        public string NLogName { get; set; }
        public DownloadByFile(string nLogName, CloudConfig cloudConfig)
        {
            NLogName = nLogName;
        }
        public bool Download(string sourcePath, string targetPath)
        {
            if (sourcePath.Length == 0) { Nlog.Info(NLogName, $"{Path.GetFileName(sourcePath) } is null"); return false; }
            if (targetPath.Length == 0) { Nlog.Info(NLogName, $"{Path.GetFileName(targetPath) } is null"); return false; }
            if (!File.Exists(sourcePath)) { Nlog.Info(NLogName, $"未在磁盘上发现文件:{sourcePath}"); return false; }
            try
            {
                File.Copy(sourcePath, targetPath, true);
            }
            catch (Exception ex)
            {
                Nlog.Info(NLogName, $"{Path.GetFileName(targetPath) } {ex.Message}");
                return false;
            }
            Nlog.Info(NLogName, $"{Path.GetFileName(targetPath) }\tdown √");
            return true;
        }
    }

    public class DownloadByCloud : IDownload
    {
        private readonly QCloudApi _api;
        private readonly CloudObjectStorageApi api;
        public string NLogName { get; set; }

        public DownloadByCloud(string nLogName, CloudConfig cloudConfig)
        {
            NLogName = nLogName;
            _api = new QCloudApi(new QCloudApi.QCloudSecret
            {
                AppId = cloudConfig.AppId,
                BucketName = cloudConfig.BucketName,
                SecretId = cloudConfig.SecretId,
                SecretKey = cloudConfig.SecretKey
            });
            api = new CloudObjectStorageApi(cloudConfig.BucketName, cloudConfig.AppId.ToString(), cloudConfig.SecretId, cloudConfig.SecretKey);
        }
        public bool Download(string sourcePath, string targetPath)
        {
            if (sourcePath.Length == 0) { Nlog.Info(NLogName, $"{Path.GetFileName(sourcePath) } is null"); return false; }
            if (targetPath.Length == 0) { Nlog.Info(NLogName, $"{Path.GetFileName(targetPath) } is null"); return false; }

            try
            {
                var result = api.GetAsync(sourcePath, targetPath);
                //var result = _api.Download(sourcePath, targetPath);
                Nlog.Info(NLogName, $"{Path.GetFileName(targetPath) }\tCloud down {(result ? "√" : "failed")}");
                return result;
            }
            catch (Exception ex)
            {
                Nlog.Info(NLogName, $"{Path.GetFileName(targetPath) } {ex.Message}");
                return false;
            }
        }
    }


    //[XmlRpcUrl("http://192.168.102.188:8888")]
    public interface IXmlRpcProxy : CookComputing.XmlRpc.IXmlRpcProxy
    {
        [XmlRpcMethod("SendMessageByYxdy")]
        string SendMessageByYxdy(string name);

        [XmlRpcMethod("SendMessageByOcr")]
        string SendMessageByOcr(string name);


        [XmlRpcMethod("SendMessageByZyqd")]
        string SendMessageByHpList(string name);
    }

    public interface IRpcProxy
    {
        string SendMessage(string callDataBaseServiceName, string name);
    }

    public class XmlRpcProxy : IRpcProxy
    {
        private readonly IXmlRpcProxy _proxy = (IXmlRpcProxy)XmlRpcProxyGen.Create(typeof(IXmlRpcProxy));

        public XmlRpcProxy(string rpcUrl)
        {
            _proxy.Url = rpcUrl;
        }
        public string SendMessage(string callDataBaseServiceName, string name)
        {
            switch (callDataBaseServiceName)
            {
                case "Define": return _proxy.SendMessageByYxdy(name);
                case "IvInput": return _proxy.SendMessageByOcr(name);
                case "HpList": return _proxy.SendMessageByHpList(name);
                default: throw new ArgumentException(nameof(callDataBaseServiceName));
            }
        }
    }


    public class ConnectionStringConfig<T> where T : class
    {
        public static string GetConnectionString(Dictionary<string, string> connectionStringDictionary)
        {
            var key = typeof(T).Name;
            return connectionStringDictionary.ContainsKey(key) ? connectionStringDictionary[key] : connectionStringDictionary["Default"];
        }
    }
}
