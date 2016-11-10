using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Core.Util
{
    public class LogManager
    {
        //<summary>  
        //保存日志的文件夹  
        //<summary>  
        private static string logPath = AppDomain.CurrentDomain.BaseDirectory;


        //<summary>  
        //写日志  
        //<summary>  
        public static void WriteLog(LogFile logFile, string msg)
        {
            WriteLog(logFile.ToString(), msg);
        }
        //<summary>  
        //写日志  
        //<summary>  
        public static void WriteLog(string logFile, string msg)
        {
            try
            {
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                        logPath + logFile + " " +
                        DateTime.Now.ToString("yyyyMMdd") + ".Log"
                    );
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:  ") + msg);
                sw.Close();
            }
            catch (Exception ee)
            {
                //System.Windows.Forms.MessageBox.Show("在LogManager类中操作WriteLog方法时异常：" + ee.Message);  
            }
        }


        public static void WriteDownLoadProcess(string message)
        {
            try
            {
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                        logPath + "result.Log"
                    );
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:  ") + message);
                sw.Close();
            }
            catch (Exception ee)
            {
            }
        }
    }
    //<summary>  
    //日志类型  
    //<summary>  
    public enum LogFile
    {
        XmlError,
        OtherError
    }
}