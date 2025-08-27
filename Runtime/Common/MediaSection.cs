using System;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    /// <summary>
    /// 媒体章节
    /// </summary>
    [Serializable]
    public class MediaSection
    {
        [SerializeField] private string _sectionName;
        [SerializeField] private double _sectionLocation;

        /// <summary>
        /// 媒体章节是有效的
        /// </summary>
        public bool mediaSectionIsValid => _sectionLocation >= 0;

        /// <summary>
        /// 媒体章节名称
        /// </summary>
        public string SectionName => _sectionName;

        /// <summary>
        /// 媒体章节位置
        /// </summary>
        public double SectionLocation => _sectionLocation;

        public MediaSection(string sectionName, int sectionLocation)
        {
            _sectionName = sectionName;
            _sectionLocation = sectionLocation;
        }
    }
}