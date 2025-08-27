using System;
using System.Threading.Tasks;
using DG.Tweening;
using Pixelsmao.UnityCommonSolution.Extensions;
using RenderHeads.Media.AVProVideo;
using RenderHeads.Media.AVProVideo.Demos;
using RenderHeads.Media.AVProVideo.Demos.UI;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0628

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    [RequireComponent(typeof(MediaPlayerUI), typeof(CanvasGroup))]
    public class MultimediaStandardPlayer : MultimediaPlayer, IMultimediaPlayerUI
    {
        internal enum PlayerUIState
        {
            Disable,
            Normal,
            Maximize,
            Minimize,
        }

        [Tooltip("是否启用控制栏")] public bool enableControlBar = true;
        [Tooltip("媒体章节选择器缩放")] public float mediaSelectorScale = 1.0f;
        [Tooltip("动画持续时间")] public float animDuration = 0.5f;
        [Tooltip("主题颜色")] public Color themeColor = new Color(0.0862f, 0.7803f, 0.4627f, 1f);
        [SerializeField] private PlayerUIState uiState = PlayerUIState.Normal;
        public RectTransform rectTransform => _rectTransform ??= GetComponent<RectTransform>();
        protected MediaPlayerUI mediaPlayerUI => _mediaPlayerUI ??= GetComponentInChildren<MediaPlayerUI>();
        public SubtitlesUGUI subtitlesUGUI => _subtitlesUGUI ??= GetComponentInChildren<SubtitlesUGUI>();
        public OverlayManager overlayManager => _overlayManager ??= GetComponentInChildren<OverlayManager>();
        private RectTransform _rectTransform;
        private MediaPlayerUI _mediaPlayerUI;
        private SubtitlesUGUI _subtitlesUGUI;
        private OverlayManager _overlayManager;

        [Header("UGUI Components")] [SerializeField]
        private RectTransform mediaSectionsTrack;

        [SerializeField] private GameObject mediaSectionPrefabs;
        [SerializeField] private Text notificationText;
        [SerializeField] private Button fullScreenButton;
        [SerializeField] private RawImage fullScreenImage;
        [SerializeField] private Texture2D fullScreenTexture2D;
        [SerializeField] private Texture2D exitFullScreenTexture2D;
        [SerializeField] private RectTransform controlBarRectTransform;
        private CanvasGroup _canvasGroup;
        private IMultimediaPlayer _multimediaPlayer;
        private Vector3 _defaultPosition;
        private Vector3 _defaultSizeDelta;
        private Vector3 _defaultScale;

        private void OnEnable()
        {
            _canvasGroup.DOFade(1, animDuration);
        }


        protected override void Start()
        {
            _rectTransform ??= GetComponent<RectTransform>();
            _mediaPlayerUI ??= GetComponentInChildren<MediaPlayerUI>();
            _subtitlesUGUI ??= GetComponentInChildren<SubtitlesUGUI>();
            _overlayManager ??= GetComponentInChildren<OverlayManager>();
            _canvasGroup ??= GetComponent<CanvasGroup>();
            audioOutput.Player ??= mediaPlayer;
            mediaPlayerUI._mediaPlayer ??= mediaPlayer;
            _multimediaPlayer = GetComponent<IMultimediaPlayer>();
            fullScreenButton.onClick.AddListener(() =>
            {
                switch (uiState)
                {
                    case PlayerUIState.Disable:
                        break;
                    case PlayerUIState.Normal:
                        Maximize();
                        break;
                    case PlayerUIState.Maximize:
                        Normal(true);
                        break;
                    case PlayerUIState.Minimize:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
            SendNotification(string.Empty);
        }


        private void Update()
        {
            UpdateControlBar();
        }

        private void OnDisable()
        {
            _canvasGroup.alpha = 0;
        }

        private void UpdateControlBar()
        {
            controlBarRectTransform.anchoredPosition =
                enableControlBar ? Vector2.zero : new Vector2(Screen.currentResolution.width * 2, 0);
        }

        public void Maximize()
        {
            if (uiState is PlayerUIState.Disable or PlayerUIState.Maximize) return;
            fullScreenImage.texture = exitFullScreenTexture2D;
            _defaultSizeDelta = rectTransform.sizeDelta;
            _defaultPosition = rectTransform.anchoredPosition;
            rectTransform.DOLocalMove(Vector3.zero, animDuration);
            rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height), animDuration).onComplete = () =>
            {
                uiState = PlayerUIState.Maximize;
            };
        }

        public void Normal(bool continuePlay)
        {
            if (uiState is PlayerUIState.Disable or PlayerUIState.Normal) return;
            fullScreenImage.texture = fullScreenTexture2D;
            rectTransform.DOLocalMove(_defaultPosition, animDuration);
            rectTransform.DOSizeDelta(_defaultSizeDelta, animDuration).onComplete = () =>
            {
                if (continuePlay && !mediaPlayer.Control.IsPlaying()) _multimediaPlayer.Play();
                uiState = PlayerUIState.Normal;
            };
        }

        public void Minimize(bool pause)
        {
            if (uiState is PlayerUIState.Disable or PlayerUIState.Minimize) return;
            _defaultScale = rectTransform.localScale;
            rectTransform.DOScale(Vector3.zero, animDuration).onComplete = () =>
            {
                if (pause) _multimediaPlayer.Pause();
                uiState = PlayerUIState.Minimize;
            };
        }

        public void Close()
        {
            _canvasGroup.DOFade(0, animDuration).onComplete = () => { gameObject.SetActive(false); };
        }

        public void CloseImmediately()
        {
            gameObject.SetActive(false);
        }

        public void HideControlBar()
        {
            MediaPlayerUI.UserInteraction.InactiveTime += mediaPlayerUI._userInactiveDuration;
        }

        public void ShowControlBar()
        {
            MediaPlayerUI.UserInteraction.InactiveTime = 0;
        }

        public void DisableControlBar()
        {
            enableControlBar = false;
        }

        public void EnableControlBar()
        {
            enableControlBar = true;
        }

        public void ToggleSubtitlesDisplay()
        {
            subtitlesUGUI.gameObject.SetActive(!subtitlesUGUI.gameObject.activeInHierarchy);
        }

        public void EnableSubtitles()
        {
            subtitlesUGUI.gameObject.SetActive(true);
        }

        public void DisableSubtitles()
        {
            subtitlesUGUI.gameObject.SetActive(false);
        }

        public void ToggleControlBarDisplay()
        {
            enableControlBar = !enableControlBar;
        }

        public void ToggleOverlayDisplay()
        {
            overlayManager.gameObject.SetActive(!overlayManager.gameObject.activeInHierarchy);
        }

        public void EnableOverlay()
        {
            overlayManager.gameObject.SetActive(true);
        }

        public void DisableOverlay()
        {
            overlayManager.gameObject.SetActive(false);
        }

        public async void SendNotification(string text)
        {
            notificationText.canvasRenderer.SetAlpha(1.0f);
            notificationText.text = text;
            await Task.Delay(TimeSpan.FromSeconds(3));
            notificationText.CrossFadeAlpha(0, animDuration, true);
        }

        protected override void UpdateMediaSections()
        {
            mediaSectionsTrack.DestroyAllChildren();
            if (!TryGetMediaReferenceInfo(out var referenceInfo)) return;
            if (mediaPlayer.MediaReference != referenceInfo.mediaReference) return;
            foreach (var section in referenceInfo.sections)
            {
                var sectionSelector =
                    Instantiate(mediaSectionPrefabs, mediaSectionsTrack).GetComponent<MediaSelector>();
                sectionSelector.SetMediaSection(mediaPlayer, section);
                sectionSelector.SetLocalScale(Vector3.one * mediaSelectorScale);
            }
        }
    }
}