using Code.Gameplay.Common.UIService;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Gameplay.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseScreen : MonoBehaviour, IScreen
    {
        [SerializeField] protected Canvas canvas;
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected float fadeDuration = 0.5f;

        public Canvas Canvas => canvas;
        public bool IsVisible { get; private set; }

        public virtual async UniTask Show()
        {
            if (!gameObject) return;
            canvas.enabled = true;
            await Fade(0f, 1f, fadeDuration);
            if (!gameObject) return;
            IsVisible = true;
        }

        public virtual async UniTask Hide()
        {
            if (!gameObject) return;
            await Fade(1f, 0f, fadeDuration);
            if (!gameObject) return;
            canvas.enabled = false;
            IsVisible = false;
        }

        private async UniTask Fade(float from, float to, float duration)
        {
            if (!gameObject || !canvasGroup) return;
            canvasGroup.alpha = from;
            float elapsed = 0f;

            while (elapsed < duration && gameObject && canvasGroup)
            {
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            if (gameObject && canvasGroup)
                canvasGroup.alpha = to;
        }

        protected virtual void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        }
    }
}