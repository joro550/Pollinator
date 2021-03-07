using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Interface
{
    public class LifeInterface : MonoBehaviour
    {
        [SerializeField] private GameObject lifeIndicatorImage;

        private readonly List<GameObject> _lifeIndicators =new List<GameObject>();
        
        public void AddLifeIndicator()
        {
            var lifeIndicator = Instantiate(lifeIndicatorImage, Vector3.zero, Quaternion.identity, this.gameObject.transform);
            _lifeIndicators.Add(lifeIndicator);
        }

        public GameObject GetObject()
        {
            return lifeIndicatorImage;
        }

        public int GetLifeCount() 
            => _lifeIndicators.Count;

        public void RemoveLifeIndicator()
        {
            var lastEntry = _lifeIndicators.Last();
            Destroy(lastEntry);
            _lifeIndicators.Remove(lastEntry);
        }
    }
}