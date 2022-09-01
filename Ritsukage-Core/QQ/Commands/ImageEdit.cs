﻿using Downloader;
using Ritsukage.Tools.Console;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using Sora.Entities.CQCodes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Ritsukage.Library.Graphic.ImageEdit;

namespace Ritsukage.QQ.Commands
{
    [CommandGroup("Image Edit"), NeedCoins(5)]
    public static class ImageEdit
    {
        static async Task<string> GetImageUrl(SoraMessage e)
        {
            var imglist = e.Message.GetAllImage();
            if (imglist.Count <= 0)
            {
                await e.ReplyToOriginal("未检测到任何图片");
                return null;
            }
            await e.ReplyToOriginal("请稍后");
            return (await e.SoraApi.GetImage(imglist.First().ImgFile)).url;
        }

        static async Task<Stream> DownloadImage(string url)
        {
            var downloader = new DownloadService(new DownloadConfiguration()
            {
                BufferBlockSize = 4096,
                ChunkCount = 5,
                OnTheFlyDownload = false,
                ParallelDownload = true
            });
            var stream = await downloader.DownloadFileTaskAsync(url);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        static async Task SendImage(SoraMessage e, Image<Rgba32> image, IImageFormat format)
        {
            var file = Path.GetTempFileName();
            SaveImage(image, format, file);
            await e.Reply(CQCode.CQImage(file));
            await e.RemoveCoins(5);
        }

        static async Task Worker(SoraMessage e, Func<Image<Rgba32>, Image<Rgba32>> func)
        {
            try
            {
                var url = await GetImageUrl(e);
                if (url == null)
                    return;
                var stream = await DownloadImage(url);
                var image = LoadImage(stream, out IImageFormat format);
                var product = func.Invoke(image);
                await SendImage(e, product, format);
            }
            catch (Exception ex)
            {
                ConsoleLog.Error("Image Edit", ex.GetFormatString());
                await e.ReplyToOriginal("因发生异常导致图像生成失败，", ex.Message);
            }
        }

        [Command("镜像左")]
        [CommandDescription("修改为左右对称的图像（左侧镜像到右侧）")]
        [ParameterDescription(1, "图像")]
        public static async void WorkMirrorLeft(SoraMessage e)
            => await Worker(e, MirrorLeft);

        [Command("镜像右")]
        [CommandDescription("修改为左右对称的图像（右侧镜像到左侧）")]
        [ParameterDescription(1, "图像")]
        public static async void WorkMirrorRight(SoraMessage e)
            => await Worker(e, MirrorRight);

        [Command("镜像上")]
        [CommandDescription("修改为上下对称的图像（上侧镜像到下侧）")]
        [ParameterDescription(1, "图像")]
        public static async void WorkMirrorTop(SoraMessage e)
            => await Worker(e, MirrorTop);

        [Command("镜像下")]
        [CommandDescription("修改为上下对称的图像（下侧镜像到上侧）")]
        [ParameterDescription(1, "图像")]
        public static async void WorkMirrorBottom(SoraMessage e)
            => await Worker(e, MirrorBottom);

        [Command("马赛克")]
        [CommandDescription("修改为马赛克处理后的图像")]
        [ParameterDescription(1, "马赛克大小", "默认值:2")]
        [ParameterDescription(2, "像素取值偏移X", "默认值:0")]
        [ParameterDescription(3, "像素取值偏移Y", "默认值:0")]
        public static async void WorkMosaic(SoraMessage e, int size = 2, int px = 0, int py = 0)
        {
            try
            {
                var url = await GetImageUrl(e);
                if (url == null)
                    return;
                var stream = await DownloadImage(url);
                var image = LoadImage(stream, out IImageFormat format);
                var product = Mosaic(image, size, px, py);
                await SendImage(e, product, format);
            }
            catch (Exception ex)
            {
                ConsoleLog.Error("Image Edit", ex.GetFormatString());
                await e.ReplyToOriginal("因发生异常导致图像生成失败，", ex.Message);
            }
        }
    }
}
