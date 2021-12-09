using System;
using Photon.Pun;
using UnityEngine;


public class TouchAndGo : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D _rb;
    private Touch _touch;
    private Vector3 _touchPosition, _whereToMove;
    private bool _isMoving = false;
    public GameObject hitParticle;
    private float _previousDistanceToTouchPos, _currentDistanceToTouchPos;
    public static event Action EndGameParticles;

    private void OnEnable()
    {
        Filler.TimeOut += LeaveRoom;
    }

    private void OnDisable()
    {
        Filler.TimeOut -= LeaveRoom;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!GameValues.IsGameStart) return;
        if (GameValues.IsGameOver) return;
        if (_isMoving)
            _currentDistanceToTouchPos = (_touchPosition - transform.position).magnitude;

        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began)
            {
                _previousDistanceToTouchPos = 0;
                _currentDistanceToTouchPos = 0;
                _isMoving = true;
                if (Camera.main is { }) _touchPosition = Camera.main.ScreenToWorldPoint(_touch.position);
                _touchPosition.z = 0;
                _whereToMove = (_touchPosition - transform.position).normalized;
                _rb.velocity = new Vector2(_whereToMove.x * moveSpeed, _whereToMove.y * moveSpeed);
            }
        }

        if (_currentDistanceToTouchPos > _previousDistanceToTouchPos)
        {
            _isMoving = false;
            _rb.velocity = Vector2.zero;
        }

        if (_isMoving)
            _previousDistanceToTouchPos = (_touchPosition - transform.position).magnitude;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("EndGame"))
        {
            AudioManager.Instance.Play(AudioManager.SoundType.Hit);
            hitParticle.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            EndGameParticles?.Invoke();
            LeaveRoom();
        }


        _isMoving = false;
        _rb.velocity = Vector2.zero;
    }

    private void LeaveRoom()
    {
        GetComponent<PhotonView>().RPC(nameof(RpcLeaveRoom), RpcTarget.All);
    }

    [PunRPC]
    public void RpcLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}