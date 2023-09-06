using Ritsukage.Tools;
using Sora.Entities.Segment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ritsukage.QQ.Commands
{
    [CommandGroup("LJYYS")]
    public static class LJYYS
    {
        [Command("ljyys")]
        [CommandDescription("ljyys")]
        public static async void Generate(SoraMessage e)
        {
            await e.ReplyToOriginal(SoraSegment.Image("/ljyys.jpg"));
        }
    }
}
