﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Util
{
    public class CSharpHelper
    {
        /// <summary>
        /// 更改代码颜色
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChangeColor(string str)
        {
            //自定义类颜色更改
            Regex userClass = new Regex(@"class (?<className>[\s\S]*?)( |\(|<)");
            Match classMatch = userClass.Match(str);
            while (classMatch.Success)
            {
                str = str.Replace(classMatch.Groups["className"].ToString(), "<font color=\"#00868B\">" + classMatch.Groups["className"] + "</font>");
                classMatch = classMatch.NextMatch();
            }
            //保留关键字
            string[] reservedKeyword = new[]
            {
                "abstract","event","new","struct","as","explicit","null","switch","base","extern","object",
                "this","bool","false","operator","throw","break","finally","out",
                "true","byte","fixed","override","try","case","float","params","typeof","catch","for","private",
                "uint","char","foreach","protected","ulong","checked","goto","public","unchecked","class","if",
                "readonly","unsafe","const","implicit","ref","ushort","continue","in","return","using","decimal",
                "int","sbyte","virtual","default","interface","sealed","volatile","where",
                "delegate","internal","short","void","do","is","sizeof","while","var",
                "double","lock","stackalloc","else","long","static","enum","namespace","string"
            };
            foreach (var keyw in reservedKeyword)
            {
                if (str.Contains(keyw))
                {
                    Regex reg = new Regex(" ?" + keyw + " ");
                    str = reg.Replace(str, " <font color=\"blue\">" + keyw + "</font> ");
                }
            }
            //系统类
            string[] objClass = AllClassName(typeof(object));
            string[] sysClass = AllClassName(typeof(object));
            string[] className = objClass.Concat(sysClass).ToArray();
            for (int i = 0; i < className.Length; i++)
            {
                if (!string.IsNullOrEmpty(className[i]) && str.Contains(className[i]))
                {
                    Regex reg = new Regex("( )?" + className[i] + "( )");
                    str = reg.Replace(str, " <font color=\"#00868B\">" + className[i] + "</font> ");
                    Regex reg2 = new Regex("( )?" + className[i] + "(<)");
                    str = reg2.Replace(str, " <font color=\"#00868B\">" + className[i] + "</font><");
                    Regex reg3 = new Regex("( )?" + className[i] + @"(\()");
                    str = reg3.Replace(str, " <font color=\"#00868B\">" + className[i] + "</font>(");
                }
            }
            //泛型
            if (str.Contains("<T>"))
            {
                str = str.Replace("<T>", "<<font color=\"#00868B\">T</font>>");
                str = str.Replace("(T", "(<font color=\"#00868B\">T</font>");
                str = str.Replace("T[", "<font color=\"#00868B\">T</font>[");
                str = str.Replace("T ", "<font color=\"#00868B\">T</font> ");
            }
            return str;
        }

        /// <summary>
        /// 获取所有系统类名称
        /// </summary>
        /// <returns></returns>
        public static string[] AllClassName(Type type)
        {
            Assembly _Assembyle = Assembly.GetAssembly(type);
            Type[] _TypeList = _Assembyle.GetTypes();
            string[] classNames = new string[_TypeList.Length + 1];
            for (int i = 0; i != _TypeList.Length; i++)
            {
                classNames[i] = _TypeList[i].Name;
            }
            return classNames;
        }
    }
}
