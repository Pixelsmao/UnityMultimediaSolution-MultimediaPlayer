using System.ComponentModel;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public class MultimediaSimplePlayer : MultimediaPlayer
    {
        public ScaleMode ScaleMode
        {
            get => displayUGUI.ScaleMode;
            set => displayUGUI.ScaleMode = value;
        }

        public bool AutoStart
        {
            get => mediaPlayer.AutoStart;
            set => mediaPlayer.AutoStart = value;
        }

        public bool AutoOpen
        {
            get => mediaPlayer.AutoOpen;
            set => mediaPlayer.AutoOpen = value;
        }

        protected override void UpdateMediaSections()
        {
            throw new WarningException($"简易多媒体播放器无需实现UpdateMediaSections方法。");
        }
    }
}