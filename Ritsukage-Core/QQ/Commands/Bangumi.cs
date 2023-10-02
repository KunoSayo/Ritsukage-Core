using Newtonsoft.Json.Linq;
using Ritsukage.Library.Data;
using Ritsukage.Library.Service;
using Ritsukage.Tools;
using Sora.Entities.Segment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ritsukage.QQ.Commands
{
    [CommandGroup("Bangumi")]
    public static class Bangumi
    {
        
        private static string GetWeekdayName(DateTime time)
        {
            int now = (int)time.DayOfWeek;
            string[] weekday = { "星期天", "星期一", "星期二", "星期三", "星期四", "星期五" , "星期六" };
            string[] weekdayJP = { "日曜日", "月曜日", "火曜日", "水曜日", "木曜日", "金曜日", "土曜日" };
            return $"{weekday[now]}({weekdayJP[now]})";
        }
        

        [Command("Bangumi今日番")]
        [CommandDescription("获取Bangumi日历")]
        public static async void BangumiCalendar(SoraMessage e)
        {
            try
            {
                var json = JArray.Parse(Utils.HttpGET("https://api.bgm.tv/calendar", ua: "bangumi"));
                // Sunday is 0
                var dayOfWeek = DateTime.Now.DayOfWeek;

                int now = (int)dayOfWeek;
                --now;
                if (now < 0)
                {

                    now += 7;
                }
                var today = json[now];
                var weekday = today["weekday"]["cn"];
                var weekdayJP = today["weekday"]["ja"];
                var reply = new StringBuilder();
                reply.AppendLine($"今天是{weekday}({weekdayJP})");

                foreach (var item in today["items"])
                {
                    string nameCN = (string)item["name_cn"];
                    reply.Append(item["name"]);
                    if (nameCN.Length > 0)
                    {
                        reply.Append($"({nameCN})");
                    }
                    reply.Append(" (").Append(item["air_date"]).AppendLine("开播) ");
                }
                await e.ReplyToOriginal(reply.ToString());

            }
            catch
            {
                await e.ReplyToOriginal("获取信息失败。");
            }
        }


        [Command("今日番")]
        [CommandDescription("获取今日番")]
        public static async void CalendarToday(SoraMessage e)
        {
            var now = DateTime.Now;
            var bs = await BangumiService.GetBangumi(now);
            var reply = new StringBuilder();
            reply.Append("今天是");
            reply.AppendLine(GetWeekdayName(now));
            foreach (var item in bs)
            {
                reply.Append(item.Title);
                if (item.ZHTitle != null)
                {
                    reply.Append('(').Append(item.ZHTitle).Append(')');
                }
                reply.Append(' ').Append(new BroadcastPeriod(item.Broadcast).Broadcast.TimeOfDay).AppendLine();
            }
            await e.ReplyToOriginal(reply.ToString());
        }

        [Command("昨日番")]
        [CommandDescription("获取昨日番")]
        public static async void CalendarYesterday(SoraMessage e)
        {
            var time = DateTime.Now.AddDays(-1);
            var bs = await BangumiService.GetBangumi(time);
            var reply = new StringBuilder();
            reply.Append("昨天是");
            reply.AppendLine(GetWeekdayName(time));
            foreach (var item in bs)
            {
                reply.Append(item.Title);
                if (item.ZHTitle != null)
                {
                    reply.Append('(').Append(item.ZHTitle).Append(')');
                }
                reply.Append(' ').Append(new BroadcastPeriod(item.Broadcast).Broadcast.TimeOfDay).AppendLine();
            }
            await e.ReplyToOriginal(reply.ToString());
        }

        [Command("明日番")]
        [CommandDescription("获取明日香")]
        public static async void CalendarTomorrow(SoraMessage e)
        {
            var time = DateTime.Now.AddDays(1);
            var bs = await BangumiService.GetBangumi(time);
            var reply = new StringBuilder();
            reply.Append("明天是");
            reply.AppendLine(GetWeekdayName(time));
            foreach (var item in bs)
            {
                reply.Append(item.Title);
                if (item.ZHTitle != null)
                {
                    reply.Append('(').Append(item.ZHTitle).Append(')');
                }
                reply.Append(' ').Append(new BroadcastPeriod(item.Broadcast).Broadcast.TimeOfDay).AppendLine();
            }
            await e.ReplyToOriginal(reply.ToString());
        }

        [Command("查询日期番剧")]
        [CommandDescription("获得指定一天的番剧信息")]
        [ParameterDescription(1, "查询日期")]
        public static async void Calander(SoraMessage e, DateTime time)
        {
            var bs = await BangumiService.GetBangumi(time);
            var reply = new StringBuilder();
            reply.Append(time.Date);
            reply.Append(" ");
            reply.AppendLine(GetWeekdayName(time));
            foreach (var item in bs)
            {
                reply.Append(item.Title);
                if (item.ZHTitle != null)
                {
                    reply.Append('(').Append(item.ZHTitle).Append(')');
                }
                reply.Append(' ').Append(new BroadcastPeriod(item.Broadcast).Broadcast.TimeOfDay).AppendLine();
            }
            await e.ReplyToOriginal(reply.ToString());
        }

        [Command("搜索番剧", "搜索番")]
        [CommandDescription("查询指定番剧")]
        [ParameterDescription(1, "番剧名称")]
        public static async void SearchBangumi(SoraMessage e, string name)
        {
            var bs = await BangumiService.SearchBangumi(name);
            var reply = new StringBuilder();
            foreach (var item in bs)
            {
                reply.Append(item.Title);
                if (item.ZHTitle != null)
                {
                    reply.Append('(').Append(item.ZHTitle).Append(')');
                    
                }
                reply.Append("(").Append(item.Begin).Append("开播)");
                reply.AppendLine();
            }
            await e.ReplyToOriginal(reply.ToString());
        }



        [Command("更新番剧信息")]
        [CommandDescription("更新数据库")]
        public static async void Refresh(SoraMessage e)
        {
            var now = DateTime.Now;
            await e.ReplyToOriginal("正在更新");
            var date = await BangumiService.RefreshBangumis(now);
            await e.ReplyToOriginal($"更新完成，更新后最晚开播番剧的日期为：{date}");
        }

    }
}
