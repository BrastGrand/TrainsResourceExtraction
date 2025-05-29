using UnityEngine;
using TMPro;
using Code.Gameplay.Services;
using Zenject;

namespace Code.Gameplay.UI
{
    public class ResourcesDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI totalResourcesText;
        [SerializeField] private TextMeshProUGUI deliveredResourcesText;

        [Inject] private IUIService _uiService;

        private int _totalResources;
        private int _deliveredResources;

        private void Start()
        {
            _uiService.RegisterResourcesDisplay(this);
            UpdateDisplay();
        }

        public void UpdateTotalResources(int totalResources)
        {
            _totalResources = totalResources;
            UpdateDisplay();
        }

        public void UpdateDeliveredResources(int deliveredResources)
        {
            _deliveredResources = deliveredResources;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (totalResourcesText != null)
                totalResourcesText.text = $"Total Resources: {_totalResources}";
        }
    }
} 