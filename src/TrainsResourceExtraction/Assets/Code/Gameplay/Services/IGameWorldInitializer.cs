using UnityEngine;

namespace Code.Gameplay.Services
{
    public interface IGameWorldInitializer
    {
        void InitializeWorld();
        void CreateTrains();
        void CreateGraphFromImage(Texture2D graphImage);
    }
} 