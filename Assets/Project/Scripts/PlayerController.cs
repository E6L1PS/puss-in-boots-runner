using System;
using System.Collections;
using UnityEngine;

namespace Project.Scripts
{
    public class PlayerController : Sounds
    {
        #region CONSTANTS

        private const float MaxSpeed = 110f;

        #endregion

        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float gravity;
        [SerializeField] private float lineDistance;
        [SerializeField] private GameObject losePanel;
        [SerializeField] private Score score;

        private bool _isSliding;
        private Vector3 _dir;
        private Animator _animator;
        private CharacterController _controller;
        private LineToMove _lineToMove = LineToMove.Middle;

        private bool IsGrounded => _controller.isGrounded;

        private enum LineToMove
        {
            Left,
            Middle,
            Right
        }

        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int Jump1 = Animator.StringToHash("jump");
        private static readonly int Slide1 = Animator.StringToHash("slide");
        private static readonly int IsFall = Animator.StringToHash("isFall");
        private static readonly int Right = Animator.StringToHash("right");
        private static readonly int Left = Animator.StringToHash("left");


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
                _animator.SetTrigger(Right);
                _lineToMove++;
            }

            if (SwipeController.swipeLeft && _lineToMove != LineToMove.Left)
            {
                _animator.SetTrigger(Left);
                _lineToMove--;
            }

            if (SwipeController.swipeUp && IsGrounded)
                Jump();

            if (SwipeController.swipeDown)
                Slide();
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
        }

        private void Jump()
        {
            PlaySound(sounds[0]);
            _dir.y = jumpForce;
            _animator.SetTrigger(Jump1);
        }

        private void Slide()
        {
            _dir.y = 0;
            _animator.SetTrigger(Slide1);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!hit.gameObject.CompareTag("obstacle")) return;
            speed = 0;
            _animator.SetBool(IsFall, true);
            PlaySound(sounds[2]);
        }

        public void AnimationCompleteEvent()
        {
            var currentCount = PlayerPrefs.GetInt("milk", 0);
            PlayerPrefs.SetInt("milk", currentCount + score.milkBottles);
            var currentScore = PlayerPrefs.GetInt("score", 0);
            var gameScore = score.score;

            PlayerPrefs.SetInt("score", gameScore > currentScore ? gameScore : currentScore);
            losePanel.SetActive(true);
            Time.timeScale = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("milk")) return;
            score.AddMilkBottle();
            PlaySound(sounds[1]);
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
}