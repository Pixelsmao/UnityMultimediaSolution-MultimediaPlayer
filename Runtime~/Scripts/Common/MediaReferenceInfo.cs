using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [Serializable]
    public class MediaReferenceInfo
    {
        public FileSystemInfo mediaFileInfo { get; }
        public FileSystemInfo mediaSegmentFileInfo { get; private set; }
        public MediaReference mediaReference { get; }
        public MediaPath mediaPath => mediaReference.MediaPath;
        public List<MediaSegment> segments = new List<MediaSegment>();

        private static readonly Regex segmentRegex =
            new Regex(@"\[(\d{2}):(\d{2}):(\d{2})\](.*)", RegexOptions.Compiled);

        public MediaReferenceInfo(FileSystemInfo fileInfo, bool useMediaSegment = false)
        {
            mediaFileInfo = fileInfo;
            mediaReference = ScriptableObject.CreateInstance<MediaReference>();
            mediaReference.MediaPath = new MediaPath(fileInfo.FullName, MediaPathType.AbsolutePathOrURL);
            if (!useMediaSegment) return;
            LoadMediaSegmentInfos();
        }

        private void LoadMediaSegmentInfos()
        {
            try
            {
                var mediaSegmentFilePath = mediaFileInfo.FullName + "-MediaSegmentInfo.ini";
                mediaSegmentFileInfo = new FileInfo(mediaSegmentFilePath);
                if (mediaSegmentFileInfo.Exists)
                {
                    var lines = File.ReadAllLines(mediaSegmentFileInfo.FullName);
                    Debug.Log(lines);
                    foreach (var line in lines.Skip(1))
                    {
                        
                        var match = segmentRegex.Match(line);
                        if (!match.Success) continue;
                        if (!int.TryParse(match.Groups[1].Value, out var hours) ||
                            !int.TryParse(match.Groups[2].Value, out var minutes) ||
                            !int.TryParse(match.Groups[3].Value, out var seconds)) continue;
                        var totalSeconds = hours * 3600 + minutes * 60 + seconds;
                        var text = match.Groups[4].Value.Trim();
                        segments.Add(new MediaSegment(text, totalSeconds));
                    }
                }
                else
                {
                    File.WriteAllText(mediaSegmentFilePath,
                        "[00:00:00]本行为媒体段落示例，不会生效，同时请勿删除此行。添加媒体分段请另起一行，中括号里面填写时间，后面跟描述，就如此段文字一样。");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"视频文件{mediaFileInfo.FullName} 媒体段落解析失败！{e.Message}");
            }
        }
    }
}