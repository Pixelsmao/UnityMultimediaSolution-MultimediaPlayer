namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public interface IMediaListPlayer 
    {
        /// <summary>
        /// 播放当前列表中的索引媒体
        /// </summary>
        /// <param name="index">当前列表的媒体索引</param>
        public void PlayIndex(int index);

        /// <summary>
        /// 播放指定索引媒体列表
        /// </summary>
        /// <param name="listIndex">媒体列表索引</param>
        public void PlayListIndex(int listIndex);

        /// <summary>
        /// 播放索引列表中的索引媒体
        /// </summary>
        /// <param name="listIndex">媒体列表索引</param>
        /// <param name="index">媒体列表中的媒体索引</param>
        public void PlayListIndexMedia(int listIndex, int index);

        /// <summary>
        /// 播放下一条媒体
        /// </summary>
        /// <param name="onlyCurrentList">仅当前媒体列表，为False时会进行夸列表播放并替换当前活动的媒体列表</param>
        public void PlayNextMedia(bool onlyCurrentList = true);

        /// <summary>
        /// 播放上一条媒体
        /// </summary>
        /// <param name="onlyCurrentList">仅当前媒体列表，为False时会进行夸列表播放并替换当前活动的媒体列表</param>
        public void PlayPreviousMedia(bool onlyCurrentList = true);

        /// <summary>
        /// 播放下一个媒体列表
        /// </summary>
        /// <param name="mediaIndex">下一个媒体列表中的媒体索引</param>
        public void PlayNextList(int mediaIndex = 0);

        /// <summary>
        /// 播放上一个媒体列表
        /// </summary>
        /// <param name="mediaIndex">下一个媒体列表中的媒体索引</param>
        public void PlayPreviousList(int mediaIndex = 0);

        /// <summary>
        /// 设置媒体列表
        /// </summary>
        /// <param name="multimediaList">多媒体列表</param>
        public void SetMultimediaList(MediaList multimediaList);

        /// <summary>
        /// 设置媒体列表并播放
        /// </summary>
        /// <param name="multimediaList">媒体引用列表</param>
        /// <param name="autoPlay">是否自动播放</param>
        /// <param name="index">自动播放媒体索引</param>
        public void SetMultimediaListAndPlay(MediaList multimediaList, bool autoPlay, int index);

        /// <summary>
        /// 切换播放模式
        /// </summary>
        public void TogglePlayMode();

        /// <summary>
        /// 设置播放模式
        /// </summary>
        /// <param name="mode">媒体播放模式</param>
        public void SetPlayMode(MediaListPlayingMode mode);

        /// <summary>
        /// 设置播放模式
        /// </summary>
        /// <param name="playingModeIndex">播放模式索引</param>
        public void SetPlayMode(int playingModeIndex);

        /// <summary>
        /// 下一个播放列表：如果返回True表示列表已经更改，如果为False则表示列表没有更改。
        /// </summary>
        /// <returns></returns>
        public bool NextMultimediaList();

        /// <summary>
        /// 上一个播放列表：如果返回True表示列表已经更改，如果为False则表示列表没有更改。
        /// </summary>
        /// <returns></returns>
        public bool PreviousMultimediaList();

        /// <summary>
        /// 获取当前媒体在媒体列表中的索引
        /// </summary>
        public int GetCurrentMediaIndex();
    }
}