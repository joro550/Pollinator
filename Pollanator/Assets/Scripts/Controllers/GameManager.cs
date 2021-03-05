using UnityEngine;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BaseController baseController;
        [SerializeField] private TMPro.TMP_Text _basePollenCount;

        public void Update()
        {
            _basePollenCount.text = 
                $"Base pollen: {baseController.GetPollenCount().ToString()}";
        }
    }
}