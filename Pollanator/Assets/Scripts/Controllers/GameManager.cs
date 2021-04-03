using UnityEngine;
using Interactables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Slider interactableSlider;
        private IInteractable _currentInteractable = null;
        private GameState _gameState;
        
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if(_instance == null) 
                    _instance = FindObjectOfType<GameManager>();

                return _instance;
            }
        }
        
        private void Awake()
        {
            if(_instance == null)
                DontDestroyOnLoad(gameObject);
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void Update()
        {
            if (_currentInteractable == null)
            {
                interactableSlider.gameObject.SetActive(false);
            }
            else
            {
                interactableSlider.gameObject.SetActive(true);
                interactableSlider.image.sprite = _currentInteractable.Image;
                interactableSlider.maxValue = _currentInteractable.Max;
                interactableSlider.value = _currentInteractable.Current;
            }
        }

        public void ResetScene() 
            => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void SetInteractable(IInteractable interactable) 
            => _currentInteractable = interactable;

        public GameState GetGameState() 
            => _gameState;

        public void SetGameState(GameState gameState) 
            => _gameState = gameState;
    }
}