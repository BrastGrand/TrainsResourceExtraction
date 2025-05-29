using UnityEngine;
using TMPro;

namespace Code.Gameplay.UI
{
    public class NodeView : MonoBehaviour
    {
        [SerializeField] private TextMeshPro nodeIdText;
        [SerializeField] private TextMeshPro multiplierText;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private int _nodeId;
        private float _multiplier;
        private bool _isMine;

        public void Initialize(int nodeId, float multiplier, bool isMine)
        {
            _nodeId = nodeId;
            _multiplier = multiplier;
            _isMine = isMine;
            UpdateDisplay();
            UpdateColor();
        }

        public void UpdateMultiplier(float newMultiplier)
        {
            _multiplier = newMultiplier;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (nodeIdText != null)
                nodeIdText.text = $"{(_isMine ? "Mine" : "Base")}:{_nodeId}";
            
            if (multiplierText != null)
            {
                multiplierText.text = $"{_multiplier:F1}x";
            }
        }

        private void UpdateColor()
        {
            if (spriteRenderer == null) return;

            Color color = _isMine ? 
                new Color(0.5f, 0f, 0.5f, 1f) : // Фиолетовый для шахт
                new Color(0f, 0.7f, 1f, 1f);    // Голубой для баз

            spriteRenderer.color = color;
        }
    }
} 