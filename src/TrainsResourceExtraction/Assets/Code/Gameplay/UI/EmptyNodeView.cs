using UnityEngine;

namespace Code.Gameplay.UI
{
    public class EmptyNodeView : MonoBehaviour
    {
        private int _nodeId;

        public void Initialize(int nodeId)
        {
            _nodeId = nodeId;
        }
    }
}