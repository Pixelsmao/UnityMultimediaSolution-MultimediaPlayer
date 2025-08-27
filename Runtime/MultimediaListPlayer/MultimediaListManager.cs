using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public class MultimediaListManager : MonoBehaviour
    {
        [Tooltip("媒体库目录来源")] public MediaSource mediaSource = MediaSource.Application;

        [Tooltip("自定义媒体库目录，仅在 mediaSource=3 时生效")]
        public string customizeMediaSource;

        [Tooltip("媒体库名称")] public string mediaSourceName = "媒体库";
        [Tooltip("使用媒体段落选择功能")] public bool enableMediaSegment;
        public string mediaSourcePath => CombineMediaSourcePath();
        public bool IsEmpty => mediaReferenceInfos.Count == 0;
        public bool loadingCompleted { get; private set; }
        private DirectoryInfo mediaDirectoryInfo;
        [SerializeField] internal List<MediaReferenceInfo> mediaReferenceInfos = new List<MediaReferenceInfo>();

        private void Start()
        {
            if (!MediaDirectoryUsable()) return;
            foreach (var file in mediaDirectoryInfo.GetFiles())
            {
                if (!file.IsSupportedVideoFile()) continue;
                if (MediaExists(file.FullName)) continue;
                var mediaReferenceInfo = new MediaReferenceInfo(file, enableMediaSegment);
                mediaReferenceInfos.Add(mediaReferenceInfo);
            }

            loadingCompleted = true;
        }


        internal bool MediaIndexValid(int index)
        {
            return index > 0 && index < mediaReferenceInfos.Count;
        }

        private bool MediaExists(string mediaPath)
        {
            return mediaReferenceInfos.Any(info => info.mediaReference.MediaPath.GetResolvedFullPath() == mediaPath);
        }

        private bool MediaDirectoryUsable()
        {
            try
            {
                mediaDirectoryInfo = new DirectoryInfo(mediaSourcePath);
                if (mediaDirectoryInfo.Exists) return true;
                mediaDirectoryInfo.Create();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"多媒体目录不可用：{e.Message}");
                return false;
            }
        }

        public string CombineMediaSourcePath()
        {
            switch (mediaSource)
            {
                case MediaSource.Invariable:
                    return String.Empty;
                case MediaSource.StreamingAssets:
                    return Path.Combine(Application.streamingAssetsPath, mediaSourceName);
                case MediaSource.Application:
                    return Path.Combine(Path.GetDirectoryName(Application.dataPath), mediaSourceName);
                case MediaSource.Customize:
                    return Path.Combine(customizeMediaSource, mediaSourceName);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}