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
        public FileSystemInfo mediaSectionFileInfo { get; private set; }
        public MediaReference mediaReference { get; }
        public MediaPath mediaPath => mediaReference.MediaPath;
        public List<MediaSection> sections = new List<MediaSection>();

        private static readonly Regex sectionRegex =
            new Regex(@"\[(\d{2}):(\d{2}):(\d{2})\](.*)", RegexOptions.Compiled);

        public MediaReferenceInfo(FileSystemInfo fileInfo, bool useMediaSection = false)
        {
            mediaFileInfo = fileInfo;
            mediaReference = ScriptableObject.CreateInstance<MediaReference>();
            mediaReference.MediaPath = new MediaPath(fileInfo.FullName, MediaPathType.AbsolutePathOrURL);
            if (!useMediaSection) return;
            LoadMediaSectionInfos();
        }

        /// <summary>
        /// 尝试获取指定索引的媒体章节
        /// </summary>
        /// <param name="index">媒体章节索引</param>
        /// <param name="mediaSection">媒体章节</param>
        /// <returns></returns>
        public bool TryGetMediaSection(int index, out MediaSection mediaSection)
        {
            mediaSection = null;
            if (index < 0 || index >= sections.Count)
            {
                Debug.LogWarning($"指定索引{index}的媒体章节无效!");
                return false;
            }

            mediaSection = sections[index];
            return true;
        }

        private void LoadMediaSectionInfos()
        {
            try
            {
                var mediaSectionFilePath = mediaFileInfo.FullName + "-MediaSectionInfo.ini";
                mediaSectionFileInfo = new FileInfo(mediaSectionFilePath);
                if (mediaSectionFileInfo.Exists)
                {
                    var lines = File.ReadAllLines(mediaSectionFileInfo.FullName);
                    Debug.Log(lines);
                    foreach (var line in lines.Skip(1))
                    {
                        var match = sectionRegex.Match(line);
                        if (!match.Success) continue;
                        if (!int.TryParse(match.Groups[1].Value, out var hours) ||
                            !int.TryParse(match.Groups[2].Value, out var minutes) ||
                            !int.TryParse(match.Groups[3].Value, out var seconds)) continue;
                        var totalSeconds = hours * 3600 + minutes * 60 + seconds;
                        var text = match.Groups[4].Value.Trim();
                        sections.Add(new MediaSection(text, totalSeconds));
                    }
                }
                else
                {
                    File.WriteAllText(mediaSectionFilePath, "[00:00:00]本行为媒体章节示例，不会生效，同时请勿删除此行。" +
                                                            "添加媒体章节请另起一行，中括号中填写时间，后面填写章节描述，就如此段文字一样。");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"视频文件{mediaFileInfo.FullName} 媒体章节解析失败！{e.Message}");
            }
        }
    }
}