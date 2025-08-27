using System;
using UnityEngine;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [Serializable]
    public class MediaSegment
    {
        [SerializeField] private string _segmentName;
        [SerializeField] private double _segmentLocation;
        public bool mediaSegmentIsValid => _segmentLocation >= 0;
        public string segmentName => _segmentName;
        public double segmentLocation => _segmentLocation;

        public MediaSegment(string segmentName, int segmentLocation)
        {
            _segmentName = segmentName;
            _segmentLocation = segmentLocation;
        }
    }
}