using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] public float speed;
    
    private Rigidbody2D _body;
    private Animator _cameraAnimator;
    private static readonly int Moving = Animator.StringToHash("IsMoving");

    private void Start()
    {
        _body = GetComponent<Rigidbody2D> ();
        _cameraAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector2 (moveHorizontal, moveVertical);

        if (IsMoving(movement))
        {
            _cameraAnimator.SetBool(Moving, true);
            _body.position += movement * speed / Time.deltaTime;
        }
        else
        {
            _cameraAnimator.SetBool(Moving, false);
        }
        
    }

    private bool IsMoving(Vector2 vector) 
        => vector.x != 0 || vector.y != 0;
}
