using Code.Gameplay.UI;

namespace Code.Gameplay.Services
{
    public interface IUIService
    {
        void RegisterResourcesDisplay(ResourcesDisplay resourcesDisplay);
        ResourcesDisplay GetResourcesDisplay();
    }
} 