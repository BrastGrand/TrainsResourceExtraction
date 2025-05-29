using UnityEngine;
using TMPro;

namespace Code.Gameplay.UI
{
    public class TrainView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro trainIdText;
        [SerializeField] private TextMeshPro speedText;
        [SerializeField] private TextMeshPro statusText;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private int _trainId;
        private float _currentSpeed;
        private string _currentStatus;

        public void Initialize(int trainId)
        {
            _trainId = trainId;
            UpdateDisplay();
        }

        public void UpdateSpeed(float speed)
        {
            _currentSpeed = speed;
            UpdateDisplay();
        }

        public void UpdateStatus(string status)
        {
            _currentStatus = status;
            UpdateDisplay();
            UpdateColor();
        }

        private void UpdateDisplay()
        {
            if (trainIdText != null)
                trainIdText.text = $"T:{_trainId}";
            
            if (speedText != null)
                speedText.text = $"Speed:{_currentSpeed:F0}";
            
            if (statusText != null)
                statusText.text = $"{_currentStatus}";
        }

        private void UpdateColor()
        {
            if (spriteRenderer == null) return;

            Color color = _currentStatus switch
            {
                "Moving" => Color.yellow,
                "Mining" => Color.red,
                "HasResource" => Color.green,
                _ => Color.white
            };

            spriteRenderer.color = color;
        }
    }
} 