using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class StockData
    {
        public int errNum { get; set; }
        public string errMsg { get; set; }
        public retData retData { get; set; }
    }

    public class retData
    {
        public List<stockinfo> stockinfo { get; set; }
        public market market { get; set; }
    }

    public class stockinfo
    {
        public string name { get; set; }
        public string code { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string OpenningPrice { get; set; }
        public string closingPrice { get; set; }
        public string currentPrice { get; set; }
        public string hPrice { get; set; }
        public string lPrice { get; set; }
        public string competitivePrice { get; set; }
        public string auctionPrice { get; set; }
        public string totalNumber { get; set; }
        public string turnover { get; set; }
        public string increase { get; set; }
        public string buyOne { get; set; }
        public string buyOnePrice { get; set; }
        public string buyTwo { get; set; }
        public string buyTwoPrice { get; set; }
        public string buyThree { get; set; }
        public string buyThreePrice { get; set; }
        public string buyFour { get; set; }
        public string buyFourPrice { get; set; }
        public string buyFive { get; set; }
        public string buyFivePrice { get; set; }
        public string sellOne { get; set; }
        public string sellOnePrice { get; set; }
        public string sellTwo { get; set; }
        public string sellTwoPrice { get; set; }
        public string sellThree { get; set; }
        public string sellThreePrice { get; set; }
        public string sellFour { get; set; }
        public string sellFourPrice { get; set; }
        public string sellFive { get; set; }
        public string sellFivePrice { get; set; }
        public string minurl { get; set; }
        public string dayurl { get; set; }
        public string weekurl { get; set; }
        public string monthurl { get; set; }

    }

    public class market
    {
        public dapan shanghai { get; set; }
        public dapan shenzhen { get; set; }
    }

    public class dapan
    {
        public string name { get; set; }
        public string curdot { get; set; }
        public string curprice { get; set; }
        public string rate { get; set; }
        public string dealnumber { get; set; }
        public string turnover { get; set; }
    }

}
