using System;
using System.Collections.Generic;
using System.IO;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public static class FileInfoExtensions
    {
        private static readonly HashSet<string> videoExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".mkv", ".wmv", ".avi", ".mpeg", ".mpg", ".rmvb", ".flv", ".mp4", ".mov"
        };

        public static bool IsVideoFile(this FileInfo fileInfo)
        {
            return videoExtensions.Contains(fileInfo.Extension.ToLower());
        }

        public static bool IsSupportedVideoFile(this FileInfo fileInfo)
        {
            return videoExtensions.Contains(fileInfo.Extension.ToLower());
        }
    }
}