using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class PlayerInterface : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private PlayerController playerController;

        public void Update()
        {
            slider.maxValue = playerController.GetPollenLimit();
            slider.value = playerController.GetPollenCount();
        }
    }
}