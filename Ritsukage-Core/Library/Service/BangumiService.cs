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

        public static async Task RefreshBangumis(DateTime now)
        {
            var json = JObject.Parse(Utils.HttpGET("https://unpkg.com/bangumi-data@0.3/dist/data.json"));
            var items = json["items"];
            var tasks = new List<Task>();
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
            }
            foreach (var task in tasks)
            {
                await task;
            }
        }

        public static async Task<List<BangumiItem>> GetTodayBangumi(DateTime now)
        {
            var items = await Database.GetArrayAsync<BangumiItem>(x => x.Begin.Date <= now.Date && (x.End == null || x.End >= now) && x.Broadcast != null);
            return items.Where(x =>
            {
                var bcp = new BroadcastPeriod(x.Broadcast);
                while (bcp.Broadcast.Date < now.Date)
                {
                    bcp.Broadcast += bcp.Time;
                }
                return bcp.Broadcast.Date == now.Date;
            }).ToList();
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
