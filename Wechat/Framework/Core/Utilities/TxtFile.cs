using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities {
    public class TxtFile {
        public static List<string> Read(string path) {
            List<string> list = new List<string>();
            //从头到尾以流的方式读出文本文件
            //该方法会一行一行读出文本
            using (System.IO.StreamReader sr = new System.IO.StreamReader(path)) {
                string str;
                while ((str = sr.ReadLine()) != null) {
                    list.Add(str);
                }
            }
            return list;
        }
        public static string ReadAllText(string path) {
            return System.IO.File.ReadAllText(path);
        }
        public static string[] ReadAllTextByLine(string path) {
            return System.IO.File.ReadAllLines(path);
        }



        public static void WriteAllText(string path, string str) {
            //如果文件不存在，则创建；存在则覆盖
            System.IO.File.WriteAllText(path, str, Encoding.UTF8);
        }

        public static void WriteLines(string path, string[] lines) {
            //如果文件不存在，则创建；存在则覆盖
            //该方法写入字符数组换行显示
            System.IO.File.WriteAllLines(path, lines, Encoding.UTF8);

        }
    }
}
