using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaSegmentSelector : MonoBehaviour
{
    public Text segmentNameText;
    public Button segmentButton;
    private RectTransform rectTransform;

    private void Start()
    {
        segmentButton = GetComponent<Button>();
        segmentNameText = GetComponentInChildren<Text>();
        
    }
}
