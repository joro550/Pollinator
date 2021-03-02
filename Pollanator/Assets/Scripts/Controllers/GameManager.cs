using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private BaseController baseController;

        [SerializeField] private TMPro.TMP_Text _playerPollenCount;
        [SerializeField] private TMPro.TMP_Text _basePollenCount;
        
        public void Update()
        {
            _playerPollenCount.text = 
                $"Player pollen: {playerController.GetPollenCount().ToString()}";
            _basePollenCount.text = 
                $"Base pollen: {baseController.GetPollenCount().ToString()}";
                
        }
    }
}