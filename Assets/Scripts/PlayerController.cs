using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region CONSTANTS

    private const float MaxSpeed = 110f;

    #endregion

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private float lineDistance;
    [SerializeField] private int countMilk;
    [SerializeField] private TextMeshProUGUI countMilkText;
    [SerializeField] private GameObject losePanel;

    private Vector3 _dir;
    private Animator _animator;
    private CharacterController _controller;

    private bool _isSliding;
    private bool IsGrounded => _controller.isGrounded;
    private LineToMove _lineToMove = LineToMove.Middle;

    private enum LineToMove
    {
        Left,
        Middle,
        Right
    }

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int Jump1 = Animator.StringToHash("jump");
    private static readonly int Slide1 = Animator.StringToHash("slide");


    #region MONO

    private void Start()
    {
        Time.timeScale = 1;
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();
        StartCoroutine(SpeedIncrease());
    }

    private void Update()
    {
        HandleSwipeInput();

        UpdateAnimatorState();

        MovePlayer();
    }

    private void FixedUpdate()
    {
        _dir.z = speed;
        _dir.y += gravity * Time.fixedDeltaTime;
        _controller.Move(_dir * Time.fixedDeltaTime);
    }

    #endregion

    private void HandleSwipeInput()
    {
        if (SwipeController.swipeRight && _lineToMove != LineToMove.Right)
        {
            _lineToMove++;
        }

        if (SwipeController.swipeLeft && _lineToMove != LineToMove.Left)
        {
            _lineToMove--;
        }

        if (SwipeController.swipeUp && IsGrounded)
        {
            Jump();
        }
    }

    private void UpdateAnimatorState()
    {
        _animator.SetBool(IsRunning, _controller.isGrounded && !_isSliding);
    }

    private void MovePlayer()
    {
        var targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        switch (_lineToMove)
        {
            case LineToMove.Left:
                targetPosition += Vector3.left * lineDistance;
                break;
            case LineToMove.Right:
                targetPosition += Vector3.right * lineDistance;
                break;
            case LineToMove.Middle:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (transform.position == targetPosition) return;

        var diff = targetPosition - transform.position;
        var moveDir = diff.normalized * (speed * Time.deltaTime);
        _controller.Move(moveDir.sqrMagnitude < diff.sqrMagnitude ? moveDir : diff);

        transform.position = targetPosition;
    }

    private void Jump()
    {
        _dir.y = jumpForce;
        _animator.SetTrigger(Jump1);
    }

    private void Slide()
    {
        _animator.SetTrigger(Slide1);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.gameObject.CompareTag("obstacle")) return;
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("milk")) return;
        countMilkText.text = (++countMilk).ToString();
        Destroy(other.gameObject);
    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(4);
        if (!(speed < MaxSpeed)) yield break;
        speed += 3;
        StartCoroutine(SpeedIncrease());
    }
}