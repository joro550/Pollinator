using UnityEngine;

namespace Controllers
{
    public class SighLine : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private PolygonCollider2D _polygonCollider;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _polygonCollider = GetComponent<PolygonCollider2D>();
        }

        public void Enable()
        {
            _spriteRenderer.enabled = true;
            _polygonCollider.enabled = true;
        }

        public void Disable()
        {
            _spriteRenderer.enabled = false;
            _polygonCollider.enabled = false;
        }
    }
}