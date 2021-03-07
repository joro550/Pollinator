using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public class PlayerInterface : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private PlayerController playerController;

        private LifeInterface _lifeInterface;
        
        public void Awake() 
            => _lifeInterface = GetComponentInChildren<LifeInterface>();

        public void Start()
        {
            
            
            for (var i = 0; i < playerController.GetLives(); i++)
                _lifeInterface.AddLifeIndicator();
        }

        public void Update()
        {
            slider.maxValue = playerController.GetPollenLimit();
            slider.value = playerController.GetPollenCount();
            
            if(_lifeInterface.GetLifeCount() > playerController.GetLives())
                _lifeInterface.RemoveLifeIndicator();
        }
    }
}