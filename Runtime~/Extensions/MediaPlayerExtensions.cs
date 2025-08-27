using RenderHeads.Media.AVProVideo;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public static class MediaPlayerExtensions
    {
        /// <summary>
        /// 是有效的媒体进度
        /// </summary>
        public static bool IsValidProgress(this MediaPlayer mediaPlayer, double progress)
        {
            return progress >= 0 && progress < mediaPlayer.Info.GetDuration();
        }

        /// <summary>
        /// 进度是有效的
        /// </summary>
        public static bool IsValidPlayRate(this MediaPlayer _, float rate)
        {
            return rate >= -4 || rate <= 4;
        }
    }
}