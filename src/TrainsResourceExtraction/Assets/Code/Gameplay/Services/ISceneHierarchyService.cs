using UnityEngine;

namespace Code.Gameplay.Services
{
    public interface ISceneHierarchyService
    {
        /// <summary>
        /// Получает контейнер для поездов, создаёт если не существует
        /// </summary>
        Transform GetTrainContainer();
        
        /// <summary>
        /// Получает контейнер для узлов графа, создаёт если не существует
        /// </summary>
        Transform GetNodesContainer();
        
        /// <summary>
        /// Получает контейнер для всех игровых объектов, создаёт если не существует
        /// </summary>
        Transform GetLevelObjectsContainer();
        
        /// <summary>
        /// Получает контейнер для системных объектов (визуализация и т.д.)
        /// </summary>
        Transform GetSystemObjectsContainer();
    }
} 