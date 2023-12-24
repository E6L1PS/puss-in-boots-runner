using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _dir;
    private Animator _animator;
    [SerializeField] private int speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;

    private bool _isSliding;
    private int _lineToMove = 1;
    public float _lineDistance = 4;
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int Jump1 = Animator.StringToHash("jump");


    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (SwipeController.swipeRight)
        {
            if (_lineToMove < 2)
            {
                _lineToMove++;
            }
        }

        if (SwipeController.swipeLeft)
        {
            if (_lineToMove > 0)
            {
                _lineToMove--;
            }
        }

        if (SwipeController.swipeUp)
        {
            if (_controller.isGrounded)
            {
                Jump();
            }
        }


        if (_controller.isGrounded && !_isSliding)
        {
            _animator.SetBool(IsRunning, true);
        }
        else
        {
            _animator.SetBool(IsRunning, false);
        }

        var targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        switch (_lineToMove)
        {
            case 0:
                targetPosition += Vector3.left * _lineDistance;
                break;
            case 2:
                targetPosition += Vector3.right * _lineDistance;
                break;
        }

        if (transform.position == targetPosition)
            return;
        var diff = targetPosition - transform.position;
        var moveDir = diff.normalized * (25 * Time.deltaTime);
        _controller.Move(moveDir.sqrMagnitude < diff.sqrMagnitude ? moveDir : diff);

        transform.position = targetPosition;
    }

    private void Jump()
    {
        _dir.y = jumpForce;
        _animator.SetTrigger(Jump1);
    }

    private void FixedUpdate()
    {
        _dir.z = speed;
        _dir.y += gravity * Time.fixedDeltaTime;
        _controller.Move(_dir * Time.fixedDeltaTime);
    }
}