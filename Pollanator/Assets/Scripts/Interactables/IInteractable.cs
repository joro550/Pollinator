using UnityEngine;
using UnityEngine.UI;

namespace Interactables
{
    public interface IInteractable
    {
        float Max { get; }
        float Current { get; }
        Sprite Image { get; }
    }
}