using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] public float speed;
    
    private Rigidbody2D _rigidBody;
    private Animator _cameraAnimator;
    private static readonly int Moving = Animator.StringToHash("IsMoving");

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D> ();
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
            _rigidBody.velocity = (movement * speed );
        }
        else
        {
            _cameraAnimator.SetBool(Moving, false);
        }
        
    }

    private static bool IsMoving(Vector2 vector) 
        => vector.x != 0 || vector.y != 0;
}
