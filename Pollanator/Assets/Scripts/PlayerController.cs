using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    private bool _canHarvest;
    
    public void Update() 
        => _renderer.color = _canHarvest ? Color.blue : Color.green;

    public void SetCanHarvest(bool canHarvest) 
        => _canHarvest = canHarvest;
}