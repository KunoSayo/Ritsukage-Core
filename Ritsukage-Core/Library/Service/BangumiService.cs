using Newtonsoft.Json.Linq;
using Ritsukage.Library.Data;
using Ritsukage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ritsukage.Library.Service
{
    public static class BangumiService
    {

        public static async Task<DateTime> RefreshBangumis(DateTime now)
        {
            var json = JObject.Parse(Utils.HttpGET("https://unpkg.com/bangumi-data@0.3/dist/data.json"));
            var items = json["items"];
            var tasks = new List<Task>();
            var latest = DateTime.MinValue;
            foreach (var item in items)
            {
                string ends = (string)item["end"];
                var bangumiItem = new BangumiItem();
                bangumiItem.Title = (string)item["title"];
                bangumiItem.Begin = DateTime.Parse((string)item["begin"]);
                bangumiItem.Broadcast = (string)item["broadcast"];
                if (ends.Length > 0)
                {
                    var dt = DateTime.Parse(ends).ToLocalTime();
                    bangumiItem.End = dt;
                }

                var trans = item["titleTranslate"];
                if (trans != null)
                {
                    var zh = trans["zh-Hans"];
                    if (zh != null)
                    {
                        foreach (var z in zh)
                        {
                            bangumiItem.ZHTitle = (string)z;
                            break;
                        }
                    }
                }
                tasks.Add(Database.InsertOrReplaceAsync(bangumiItem));
                if (bangumiItem.Begin > latest)
                {
                    latest = bangumiItem.Begin;
                }
            }
            foreach (var task in tasks)
            {
                await task;
            }
            return latest;
        }
        /// <summary>
        /// 获得某一天的番剧播出信息，按播出时间排序
        /// </summary>
        /// <param name="now">查询的时间</param>
        /// <returns>排序好了的番剧信息</returns>
        public static async Task<List<BangumiItem>> GetBangumi(DateTime now)
        {
            var items = await Database.GetArrayAsync<BangumiItem>(x => x.Begin.Date <= now.Date && (x.End == null || x.End >= now) && x.Broadcast != null);
            return items
                .Select(x => (x, new BroadcastPeriod(x.Broadcast)))
                .Where(x =>
                {
                    var bcp = x.Item2;
                    while (bcp.Broadcast.Date < now.Date)
                    {
                        bcp.Broadcast += bcp.Time;
                    }
                    return bcp.Broadcast.Date == now.Date;
                })
                .OrderBy(x => x.Item2.Broadcast.TimeOfDay)
                .Select(x => x.x)
                .ToList();
        }

        /// <summary>
        /// 搜索番剧信息，上限20个，按照播出时间优先返回
        /// </summary>
        /// <param name="name">番剧名字</param>
        /// <returns>包含符合名字的番剧信息，上限20个</returns>
        public static async Task<List<BangumiItem>> SearchBangumi(string name)
        {
            var items = await Database.GetArrayAsync<BangumiItem>(x => x.Title.Contains(name) || (x.ZHTitle != null && x.ZHTitle.Contains(name)));
            return items
                .OrderByDescending(x => x.Begin)
                .Take(20)
                .ToList();
        }
    }

    public struct BroadcastPeriod
    {
        public DateTime Broadcast { get; set; }
        public TimeSpan Time { get; set; }

        public BroadcastPeriod(string s)
        {
            int last = s.LastIndexOf("/");
            Broadcast = DateTime.Parse(s[2..last]);
            int number = int.Parse(s[(last + 2)..^1]);
            switch (s[^1])
            {
                case 'M':
                    {
                        Time = TimeSpan.FromDays(number * 30);
                        break;
                    }
                default:
                    {
                        Time = TimeSpan.FromDays(number);
                        break;
                    }
            }
        }
    }
}
