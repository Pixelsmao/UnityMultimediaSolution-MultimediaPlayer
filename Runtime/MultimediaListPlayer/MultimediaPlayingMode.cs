namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    /// <summary>
    /// 多媒体循环模式
    /// </summary>
    public enum MultimediaPlayingMode
    {
        /// <summary>
        /// 单曲播放
        /// </summary>
        Once,

        /// <summary>
        /// 单曲循环
        /// </summary>
        Loop,

        /// <summary>
        /// 列表循环
        /// </summary>
        ListLoop,

        /// <summary>
        /// 跨列表循环
        /// </summary>
        CrossListLoop,

        /// <summary>
        /// 列表随机播放
        /// </summary>
        ListRandom,

        /// <summary>
        /// 跨列表随机播放
        /// </summary>
        CrossListRandom
    }
}