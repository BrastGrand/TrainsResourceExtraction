using Code.Gameplay.UI;

namespace Code.Gameplay.Services
{
    public class UIService : IUIService
    {
        private ResourcesDisplay _resourcesDisplay;

        public void RegisterResourcesDisplay(ResourcesDisplay resourcesDisplay)
        {
            _resourcesDisplay = resourcesDisplay;
        }

        public ResourcesDisplay GetResourcesDisplay()
        {
            return _resourcesDisplay;
        }
    }
} 