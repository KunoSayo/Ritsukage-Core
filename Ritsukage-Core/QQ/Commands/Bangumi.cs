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
            var bs = await BangumiService.GetTodayBangumi(DateTime.Now);
            var reply = new StringBuilder();
            foreach (var item in bs)
            {
                reply.Append(item.Title);
                if (item.Title != null)
                {
                    reply.Append(' ').Append(item.Title).Append(' ');
                }
                reply.Append(' ').Append(new BroadcastPeriod(item.Broadcast).Broadcast.TimeOfDay).AppendLine();
            }
            await e.ReplyToOriginal(reply.ToString());
        }

        [Command("昨日番")]
        [CommandDescription("获取昨日番")]
        public static async void CalendarYesterday(SoraMessage e)
        {
            var bs = await BangumiService.GetTodayBangumi(DateTime.Now.AddDays(-1));
            var reply = new StringBuilder();
            foreach (var item in bs)
            {
                reply.Append(item.Title);
                if (item.Title != null)
                {
                    reply.Append(' ').Append(item.Title).Append(' ');
                }
                reply.Append(' ').Append(new BroadcastPeriod(item.Broadcast).Broadcast.TimeOfDay).AppendLine();
            }
            await e.ReplyToOriginal(reply.ToString());
        }

        [Command("明日番")]
        [CommandDescription("获取明日香")]
        public static async void CalendarTomorrow(SoraMessage e)
        {
            var bs = await BangumiService.GetTodayBangumi(DateTime.Now.AddDays(1));
            var reply = new StringBuilder();
            foreach (var item in bs)
            {
                reply.Append(item.Title);
                if (item.Title != null)
                {
                    reply.Append(' ').Append(item.Title).Append(' ');
                }
                reply.Append(' ').Append(new BroadcastPeriod(item.Broadcast).Broadcast.TimeOfDay).AppendLine();
            }
            await e.ReplyToOriginal(reply.ToString());
        }

        
        
        [Command("更新番剧信息")]
        [CommandDescription("执行lua代码")]
        [ParameterDescription(1, "代码")]
        public static async void Refresh(SoraMessage e)
        {
            var now = DateTime.Now;
            await e.ReplyToOriginal("正在更新");
            await BangumiService.RefreshBangumis(now);
            await e.ReplyToOriginal("更新完成");
        }

    }
}
