using System.Collections.Generic;
using System.IO;
using System.Linq;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public class MediaReferenceCollections
    {
        public bool listIsEmpty => mediaReferences.Count == 0;
        internal static List<MediaReferenceInfo> mediaReferences = new List<MediaReferenceInfo>();

        public MediaReferenceCollections()
        {
        }

        public MediaReferenceCollections(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                if (!file.IsSupportedVideoFile()) return;
                if (MediaIsExist(file.FullName)) return;
                var media = ScriptableObject.CreateInstance<MediaReference>();
                media.MediaPath = new MediaPath(file.FullName, MediaPathType.AbsolutePathOrURL);
                // var mediaDetail = new MediaReferenceInfo
                // {
                //     mediaReference = media
                // };
                //mediaReferences.Add(mediaDetail);
            }
        }

        public MediaReferenceCollections(List<MediaReference> directory)
        {
        }

        private static bool MediaIsExist(string mediaPath)
        {
            return mediaReferences.Any(reference =>
                reference.mediaReference.MediaPath.GetResolvedFullPath() == mediaPath);
        }
    }
}