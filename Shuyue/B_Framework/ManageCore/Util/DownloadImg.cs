using System.Net;
using System.IO;

namespace Core.Util
{
    public class DownloadImg
    {
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
