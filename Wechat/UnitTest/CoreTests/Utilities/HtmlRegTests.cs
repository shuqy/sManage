using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Tests {
    [TestClass()]
    public class HtmlRegTests {
        [TestMethod()]
        public void RemoveLinkTest() {
            HtmlReg.RemoveLink("<img src=\"/Img/fc4814890fe244acc09596.jpg\" data-rawwidth=\"640\" data-rawheight=\"856\" class=\"origin_image zh - lightbox-thumb\" width=\"640\" data-original=\"https://pic3.zhimg.com/6662b2f3217d2ba60a8d2052739387de_r.jpg\"><a><b>safddsf</b>---------------------------------------------");
        }

        [TestMethod()]
        public void RemoveEditDateLinkTest() {
            var abc = HtmlReg.RemoveEditDateLink("<span class=\"answer-date-link-wrap\"><a class=\"answer-date-link last_updated meta-item\" data-tip=\"s$t$发布于 2015-12-15\" target=\"_blank\" href=\"/question/22425541/answer/76887168\">编辑于 2016-01-06</a></span>");
        }
    }
}