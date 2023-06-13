using Newtonsoft.Json.Linq;
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
        [Command("今日番")]
        [CommandDescription("获取Bangumi日历")]
        public static async void Calendar(SoraMessage e)
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
    }
}