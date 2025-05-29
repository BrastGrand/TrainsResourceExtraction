using Code.Gameplay.Features.ResourceManagement.Systems;
using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.ResourceManagement
{
    public class ResourceManagementFeature : Feature
    {
        public ResourceManagementFeature(ISystemFactory systems)
        {
            Add(systems.Create<UpdateTotalResourcesSystem>());
            Add(systems.Create<UpdateResourcesUISystem>());
        }
    }
}