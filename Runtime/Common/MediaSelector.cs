using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Pixelsmao.UnityCommonSolution.Extensions;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Pixelsmao.UnityMultimediaSolution.MultimediaPlayer
{
    public class MediaSelector : MonoBehaviour
    {
        public MediaSection mediaSection;
        //public float percent = 0.1f;
        private MediaPlayer _mediaPlayer;
        private Button _sectionButton;
        private Text _sectionNameText;
        private RectTransform textRectTransform => _sectionNameText.rectTransform;
        private RectTransform _parent;
        private RectTransform _rectTransform;
        public RectTransform rectTransform => _rectTransform ??= GetComponent<RectTransform>();
        private CanvasGroup _canvasGroup;
        private TweenerCore<float, float, FloatOptions> _fadeTween;

        private void Start()
        {
            _sectionButton = GetComponentInChildren<Button>();
            _sectionNameText = GetComponentInChildren<Text>();
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _parent = _rectTransform.parent as RectTransform;
            _sectionButton.onClick.AddListener(() =>
            {
                if (_fadeTween == null || !_fadeTween.IsPlaying())
                {
                    _canvasGroup.alpha = 1;
                    _fadeTween = _canvasGroup.DOFade(0, 0.5f).SetDelay(1.5f);
                    return;
                }

                if (_fadeTween == null || !_fadeTween.IsPlaying()) return;
                if (_mediaPlayer != null && mediaSection != null)
                    _mediaPlayer.Control.Seek(mediaSection.SectionLocation);
            });
        }

        private void Update()
        {
            if (_mediaPlayer == null || mediaSection == null) return;
            var percentPosition = mediaSection.SectionLocation / _mediaPlayer.Info.GetDuration();
            rectTransform.anchoredPosition = new Vector2(_parent.rect.width * (float)percentPosition, 0);
            if (rectTransform.anchoredPosition.x < 210)
            {
                textRectTransform.anchoredPosition = new Vector2(210 - rectTransform.anchoredPosition.x, 50);
            }
            else if (rectTransform.anchoredPosition.x > _parent.rect.width - 210)
            {
                //TODO 精准计算差值以使得文本紧贴边缘
                textRectTransform.anchoredPosition = new Vector2(-210, 50);
            }
            else
            {
                textRectTransform.anchoredPosition = new Vector2(0, 50);
            }
        }

        public void SetMediaSection(MediaPlayer mediaPlayer, MediaSection section)
        {
            _mediaPlayer = mediaPlayer;
            this.mediaSection = section;
        }

        public void SetLocalScale(Vector3 scale)
        {
            rectTransform.localScale = scale;
        }
    }
}