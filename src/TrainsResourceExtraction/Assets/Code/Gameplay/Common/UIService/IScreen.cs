using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Gameplay.Common.UIService
{
    public interface IScreen
    {
        Canvas Canvas { get; }
        UniTask Show();
        UniTask Hide();
        bool IsVisible { get; }
    }
}