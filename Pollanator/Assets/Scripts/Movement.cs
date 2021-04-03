using Controllers;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private Animator characterAnimator;
    
    private Rigidbody2D _rigidBody;
    private Animator _cameraAnimator;
    private PlayerController _playerController;
    
    private static readonly int Moving = Animator.StringToHash("IsMoving");
    private static readonly int FlyUp = Animator.StringToHash("FlyUp");
    private static readonly int FlyDown = Animator.StringToHash("FlyDown");
    private static readonly int FlyLeft = Animator.StringToHash("FlyLeft");
    private static readonly int FlyRight = Animator.StringToHash("FlyRight");

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D> ();
        _cameraAnimator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        PlayAnimation(moveHorizontal, moveVertical);

        var movement = new Vector2 (moveHorizontal, moveVertical);

        if (IsMoving(movement))
        {
            var pollenCount = _playerController.GetPollenCount();
            
            _cameraAnimator.SetBool(Moving, true);
            var movementSpeed =  movement * speed;
            if (pollenCount > 0) 
                movementSpeed = movement * ((speed / pollenCount) * 70);
            
            print(movementSpeed);
            _rigidBody.velocity = movementSpeed;
        }
        else
        {
            _cameraAnimator.SetBool(Moving, false);
        }
    }

    private void PlayAnimation(float horizontal, float vertical)
    {
        characterAnimator.SetBool(FlyUp, false);
        characterAnimator.SetBool(FlyDown, false);
        characterAnimator.SetBool(FlyLeft, false);
        characterAnimator.SetBool(FlyRight, false);

        if (horizontal < 0)
        {
            characterAnimator.SetBool(FlyLeft, true);
        }
        else if (horizontal > 0)
        {
            characterAnimator.SetBool(FlyRight, true);
        }

        if (vertical < 0)
        {
            characterAnimator.SetBool(FlyDown, true);
        }
        else if (vertical > 0)
        {
            characterAnimator.SetBool(FlyUp, true);
        }
    }

    private static bool IsMoving(Vector2 vector) 
        => vector.x != 0 || vector.y != 0;
}
