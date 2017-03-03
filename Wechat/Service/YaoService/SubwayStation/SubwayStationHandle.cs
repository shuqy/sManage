using Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.SubwayStation {
    public class SubwayStationHandle {
        //地铁站点文件路径
        private static readonly string subwayStationsPath;
        //地铁站点相连关系文件路径
        private static readonly string linkedSubwayPath;
        //地铁站点对应地铁线文件路径
        private static readonly string stationLinesPath;
        //所有地铁站点路径
        private static readonly string stationsPath;

        //地铁站点符号图
        private static SymbolGraph SymbolGraph;
        //地铁站点对应地铁线字典
        private static Dictionary<string, string> stationLinesDic;
        static SubwayStationHandle() {
            subwayStationsPath = @"G:\Qipa\SubwayStations.txt";
            linkedSubwayPath = @"D:\workspace\Wechat\Qipa\LinkedSubwayStations.txt";
            stationLinesPath = @"D:\workspace\Wechat\Qipa\StationLines.txt";
            stationsPath = @"D:\workspace\Wechat\Qipa\Stations.txt";
            stationLinesDic = GetStationLines(stationLinesPath);
            SymbolGraph = GetGraph(linkedSubwayPath);
        }
        /// <summary>
        /// 获取两站之间路径
        /// </summary>
        /// <param name="stationOne"></param>
        /// <param name="stationTwo"></param>
        /// <returns></returns>
        public static List<ResultPath> Path(string stationOne, string stationTwo) {
            if (SymbolGraph == null)
                SymbolGraph = GetGraph(linkedSubwayPath);
            Queue<string> path = SymbolGraph.BreadthFirstPath(stationOne, stationTwo);
            if (path == null || !path.Any()) return new List<ResultPath>();
            List<Stations> hasLinePath = PathStationAddLine(path);
            List<ResultPath> resPath = GetResultPath(hasLinePath);
            return resPath;
        }

        /// <summary>
        /// 对路径上站点再加工
        /// </summary>
        /// <param name="hasLinePath"></param>
        /// <returns></returns>
        private static List<ResultPath> GetResultPath(List<Stations> hasLinePath) {
            List<ResultPath> resPath = new List<ResultPath>();
            //hasLinePath.GroupBy(s => s.Lines).Select(s => new ResultPath { Line = s.Key, Stations = s.ToList() }).ToList();
            string thisLine = "";
            string previousLine = "";
            string nextLine = "";
            string currentLine = "";
            for (int i = 0; i < hasLinePath.Count(); i++) {
                thisLine = hasLinePath[i].Lines;
                previousLine = hasLinePath[(i - 1) < 0 ? i : (i - 1)].Lines;
                nextLine = hasLinePath[(i + 1) >= hasLinePath.Count() ? i : (i + 1)].Lines;
                currentLine = GetThisLineBelong(thisLine, nextLine);
                if (!resPath.Any(p => p.Line.Equals(currentLine))) resPath.Add(new ResultPath { Line = currentLine, Stations = new List<Stations>() });
                var line = resPath.FirstOrDefault(p => p.Line.Equals(currentLine));
                line.Stations.Add(hasLinePath[i]);
            }
            return resPath;
        }

        /// <summary>
        /// 获取当前站点所属路线
        /// </summary>
        /// <param name="thisline"></param>
        /// <param name="nextline"></param>
        /// <returns></returns>
        private static string GetThisLineBelong(string thisline, string nextline) {
            string[] lines = thisline.Split('/');
            foreach (var one in lines) {
                if (nextline.Contains(one))
                    return one;
            }
            return lines[0];
        }

        /// <summary>
        /// 路径上站点处理，加线
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static List<Stations> PathStationAddLine(Queue<string> path) {
            List<Stations> sp = new List<Stations>();
            foreach (var station in path) {
                sp.Add(new Stations { Name = station, Lines = stationLinesDic[station] });
            }
            return sp;
        }


        /// <summary>
        /// 两点是否联通
        /// </summary>
        /// <param name="stationOne"></param>
        /// <param name="stationTwo"></param>
        /// <returns></returns>
        public static bool HasPath(string stationOne, string stationTwo) {
            return SymbolGraph.HasPath(stationOne, stationTwo);
        }

        /// <summary>
        /// 获取图
        /// </summary>
        private static SymbolGraph GetGraph(string path) {
            if (!System.IO.File.Exists(path)) return null;
            string[] list = TxtFile.ReadAllTextByLine(path);
            return new SymbolGraph(list, '-');
        }

        public static string[] GetAllStations() {
            if (!System.IO.File.Exists(stationsPath)) return null;
            string stations = TxtFile.ReadAllText(stationsPath);
            return stations.Split(' ');
        }

        public static void CreateAllStationFile() {
            if (!System.IO.File.Exists(subwayStationsPath)) return;
            string[] list = TxtFile.ReadAllTextByLine(subwayStationsPath);
            string stationList = "";
            foreach (string stationstr in list) {
                if (string.IsNullOrEmpty(stationstr)) continue;
                //某条线所有站点
                string[] stations = stationstr.Split(' ');
                for (int i = 1; i < stations.Length; i++)
                    if (!stationList.Contains(stations[i]))
                        stationList += stations[i] + " ";
            }
            TxtFile.WriteAllText(stationsPath, stationList);
        }

        /// <summary>
        /// 获取站点线
        /// </summary>
        /// <param name="stationLinesPath"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetStationLines(string stationLinesPath) {
            if (!System.IO.File.Exists(stationLinesPath)) return null;
            string[] list = TxtFile.ReadAllTextByLine(stationLinesPath);
            var dic = new Dictionary<string, string>();
            foreach (var stationlines in list) {
                var sl = stationlines.Split('-');
                dic.Add(sl[0], sl[1]);
            }
            return dic;
        }

        /// <summary>
        /// 根据每条线的站点，链接相邻的站点
        /// </summary>
        /// <param name="zhandian"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private static List<string> GetLinkStations(string path) {
            if (!System.IO.File.Exists(path)) return null;
            string[] list = TxtFile.ReadAllTextByLine(path);
            List<string> linkedStationList = new List<string>();
            foreach (string stationstr in list) {
                if (string.IsNullOrEmpty(stationstr)) {
                    linkedStationList.Add("");
                    continue;
                }
                //某条线所有站点
                string[] stations = stationstr.Split(' ');
                //第一个为站点名称
                for (int i = 1; i < stations.Length - 1; i++) {
                    string linkedstation = string.Format("{0}-{1}", stations[i], stations[i + 1]);
                    linkedStationList.Add(linkedstation);
                }
            }
            return linkedStationList;
        }

        /// <summary>
        /// 根据站点生成相连通的站点路径文件
        /// </summary>
        public static void HandleStation() {
            var list = GetLinkStations(subwayStationsPath);
            TxtFile.WriteLines(linkedSubwayPath, list.ToArray());
        }

        /// <summary>
        /// 生成地铁站点对应地铁线文件
        /// </summary>
        public static void CreateStationLines() {
            if (!System.IO.File.Exists(subwayStationsPath)) return;
            string[] list = TxtFile.ReadAllTextByLine(subwayStationsPath);
            Dictionary<string, string> stationLines = new Dictionary<string, string>();
            foreach (var stationStr in list) {
                if (string.IsNullOrEmpty(stationStr)) continue;
                //某条线所有站点
                string[] stations = stationStr.Split(' ');
                //其中第一个为站点名称
                string lineName = stations[0];
                for (int i = 1; i < stations.Length; i++) {
                    if (!stationLines.ContainsKey(stations[i]))
                        stationLines.Add(stations[i], lineName);
                    else
                        stationLines[stations[i]] = string.Format("{0}/{1}", stationLines[stations[i]], lineName);
                }
            }
            string[] stationLinesArray = new string[stationLines.Count()];
            int j = 0;
            foreach (var sl in stationLines) {
                stationLinesArray[j] = string.Format("{0}-{1}", sl.Key, sl.Value);
                j++;
            }
            TxtFile.WriteLines(stationLinesPath, stationLinesArray);
        }

        public class ResultPath {
            public string Line { get; set; }
            public List<Stations> Stations { get; set; }
        }
        public class Stations {
            public string Name { get; set; }
            public string Lines { get; set; }
        }
    }
}
