using System;
using System.Collections.Generic;
using DG.Tweening;
using RenderHeads.Media.AVProVideo;
using RenderHeads.Media.AVProVideo.Demos;
using RenderHeads.Media.AVProVideo.Demos.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MediaListPlayerUI : MonoBehaviour, IMediaListPlayerUI
    {
        internal enum PlayerUIState
        {
            Disable,
            Normal,
            Maximize,
            Minimize,
        }

        public RectTransform rectTransform { get; private set; }

        [Header("UGUI Components")] [SerializeField]
        private MediaPlayer _mediaPlayer;

        [SerializeField] private MediaPlayerUI _mediaPlayerUI;
        [SerializeField] private DisplayUGUI _displayUGUI;
        [SerializeField] private SubtitlesUGUI _subtitlesUGUI;
        [SerializeField] private OverlayManager _overlayManager;
        [SerializeField] private RectTransform _mediaSegmentTrack;
        [SerializeField] private Text _notificationText;
        [SerializeField] private RectTransform mediaSegmentParent;
        [SerializeField] private PlayerUIState uiState = PlayerUIState.Normal;
        [SerializeField] private RawImage fullScreenRawImage;
        [SerializeField] private Texture2D fullScreenIcon;
        [SerializeField] private Texture2D exitFullScreenIcon;
        [SerializeField] private Texture2D mediaSegmentSignIcon;
        [SerializeField] private GameObject controlBar;
        [SerializeField] private CanvasGroup controlBarCanvasGroup;
        [Header("Settings")] public bool enableControlBar = true;
        public Vector2 displayCanvasScale = Vector2.one;
        public ScaleMode displayCanvasScaleMode = ScaleMode.StretchToFill;
        public float mediaSegmentSelectorScale = 1.0f;
        public float animDuration = 0.5f;
        public MediaPlayer mediaPlayer => _mediaPlayer;
        public DisplayUGUI displayUGUI => _displayUGUI;
        public MediaPlayerUI mediaPlayerUI => _mediaPlayerUI;
        public OverlayManager overlayManager => _overlayManager;
        public SubtitlesUGUI subtitlesUGUI => _subtitlesUGUI;
        private CanvasGroup canvasGroup;
        private IMultimediaPlayer multimediaPlayer;
        private Vector3 defaultPosition;
        private Vector3 defaultSizeDelta;
        private Vector3 defaultScale;


        protected virtual void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            multimediaPlayer = GetComponent<IMultimediaPlayer>();
            canvasGroup = GetComponent<CanvasGroup>();
            defaultScale = rectTransform.localScale;
        }

        private void OnEnable()
        {
            canvasGroup.DOFade(1, animDuration);
        }

        public void DisableUI()
        {
            throw new NotImplementedException();
        }

        public void Maximize()
        {
            if (uiState is PlayerUIState.Disable or PlayerUIState.Maximize) return;
            defaultSizeDelta = rectTransform.sizeDelta;
            defaultPosition = rectTransform.anchoredPosition;
            rectTransform.DOLocalMove(Vector3.zero, animDuration);
            rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height), animDuration).onComplete = () =>
            {
                fullScreenRawImage.texture = exitFullScreenIcon;
                uiState = PlayerUIState.Maximize;
            };
        }

        public void Normal(bool continuePlay)
        {
            if (uiState is PlayerUIState.Disable or PlayerUIState.Normal) return;
            rectTransform.DOLocalMove(defaultPosition, animDuration);
            rectTransform.DOSizeDelta(defaultSizeDelta, animDuration).onComplete = () =>
            {
                if (continuePlay) multimediaPlayer.Play();
                fullScreenRawImage.texture = fullScreenIcon;
                uiState = PlayerUIState.Normal;
            };
        }

        public void Minimize(bool pause)
        {
            if (uiState is PlayerUIState.Disable or PlayerUIState.Minimize) return;
            defaultScale = rectTransform.localScale;
            rectTransform.DOScale(Vector3.zero, animDuration).onComplete = () =>
            {
                if (pause) multimediaPlayer.Pause();
                uiState = PlayerUIState.Minimize;
            };
        }

        public void Close()
        {
            canvasGroup.DOFade(0, animDuration).onComplete = () => { gameObject.SetActive(false); };
        }

        public void CloseImmediately()
        {
            gameObject.SetActive(false);
        }

        [ContextMenu("HideControlBar")]
        public void HideControlBar()
        {
            MediaPlayerUI.UserInteraction.InactiveTime += mediaPlayerUI._userInactiveDuration;
            //controlBarCanvasGroup.DOFade(0, animDuration).onComplete = DisableControlBar;
        }

        [ContextMenu("ShowControlBar")]
        public void ShowControlBar()
        {
            MediaPlayerUI.UserInteraction.InactiveTime = 0;
            //EnableControlBar();
            //controlBarCanvasGroup.DOFade(1, animDuration);
        }

        [ContextMenu("DisableControlBar")]
        public void DisableControlBar()
        {
            controlBar.SetActive(false);
        }

        [ContextMenu("EnableControlBar")]
        public void EnableControlBar()
        {
            controlBar.SetActive(true);
        }


        public void ToggleSubtitlesDisplay()
        {
            throw new NotImplementedException();
        }

        public void EnableSubtitles()
        {
            throw new NotImplementedException();
        }

        public void DisableSubtitles()
        {
            throw new NotImplementedException();
        }

        public void ToggleControlBarDisplay()
        {
            throw new NotImplementedException();
        }

        public void ToggleOverlayDisplay()
        {
            throw new NotImplementedException();
        }

        public void EnableOverlay()
        {
            throw new NotImplementedException();
        }

        public void DisableOverlay()
        {
            throw new NotImplementedException();
        }

        public void SendNotification(string text)
        {
            throw new NotImplementedException();
        }

        public void UpdateMediaSegments(List<MediaSegment> mediaSegments, IMultimediaPlayer player)
        {
            throw new NotImplementedException();
        }

        public void UpdateMediaSegments(List<MediaSegment> mediaSegments, IMediaPlayer player)
        {
            foreach (var mediaSegment in mediaSegments)
            {
                // var sectionSelector =
                //     Instantiate(Resources.Load<GameObject>("MediaSectionSelector"), uguiControl.mediaSectionSelector)
                //         .GetComponent<MediaSectionSelector>();
                // sectionSelector.rectTransform.localScale = Vector3.one * mediaSectionSelectorScale;
                // sectionSelector.InjectionRequirement(mediaSection, this);
            }
        }
    }
}