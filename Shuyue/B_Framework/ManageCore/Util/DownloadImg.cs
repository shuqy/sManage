using System.Net;
using System.IO;

namespace Core.Util
{
    public class DownloadImg
    {
        /// <summary>
        /// 获取服务器文件路径
        /// </summary>
        /// <param name="imgName"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string GetFileServerUrl(string imgName, string folderName)
        {
            return $"{ConfigHelper.Get("ImgServerPath")}{folderName}/{imgName}.jpg";
        }
        /// <summary>
        /// 获取文件物理路径
        /// </summary>
        /// <param name="imgName"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string GetFilePath(string imgName, string folderName)
        {
            return $"{ConfigHelper.Get("ImgPath")}{folderName}\\{imgName}.jpg";
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="imgName"></param>
        /// <param name="folderName"></param>
        public static void Load(string url,string imgName,string folderName)
        {
            //如果不存在就创建file文件夹
            if (Directory.Exists($"{ConfigHelper.Get("ImgPath")}{folderName}") == false)
            {
                Directory.CreateDirectory($"{ConfigHelper.Get("ImgPath")}{folderName}");
            }
            Load(url, GetFilePath(imgName,folderName));
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="imgPath"></param>
        public static void Load(string url, string imgPath)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream reader = response.GetResponseStream();
            FileStream writer = new FileStream(imgPath, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buff = new byte[4096];
            int c = 0; //实际读取的字节数
            while ((c = reader.Read(buff, 0, buff.Length)) > 0)
            {
                writer.Write(buff, 0, c);
            }
            writer.Close();
            writer.Dispose();
            reader.Close();
            reader.Dispose();
            response.Close();
        }
    }
}
