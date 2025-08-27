using System.Collections;
using System.Collections.Generic;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public static class IListExtensions
    {
        /// <summary>
        /// 是有效的索引
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public static bool IsValidIndex(this IList list, int index)
        {
            return index > 0 && index < list.Count;
        }

        /// <summary>
        /// 是无效的索引
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public static bool IsInvalidIndex(this IList list, int index)
        {
            return !list.IsValidIndex(index);
        }
    }
}