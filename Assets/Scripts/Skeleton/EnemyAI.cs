using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Adventure.Utils;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    private float _chasingSpeedMultiplayer = 2f;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTime;
    private Vector3 _roamingPosition;
    private Vector3 _startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    

    private enum State
    {
        Idel,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;

        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplayer;
    }


    private void Update()
    {
        StateHandle();
    }

    private void StateHandle()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTime -= Time.deltaTime;
                if (_roamingTime < 0)
                {
                    Roaming();
                    _roamingTime = _roamingTimerMax;
                }
                SetCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                SetCurrentState();

                break;
            case State.Attacking: break;
            case State.Death: break;
            default:
            case State.Idel: break;
        }
    }

    private void SetCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (_isChasingEnemy)
        {
            if(distanceToPlayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }
        
        if(newState != _currentState)
        {
            if(newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            } else if(newState == State.Roaming)
            {
                _roamingTime = 0f;
                _navMeshAgent.speed = _roamingSpeed;
            }
            _currentState = newState;
        }
        

    }

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    public bool IsRunning()
    {
        if(_navMeshAgent.velocity == Vector3.zero)
        {
            return false;
        }else
        {
            return true;
        }
    }

    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamingPosition = GetRoamingPosition();
        ChangeFacingDirection(_startingPosition, _roamingPosition);
        _navMeshAgent.SetDestination(_roamingPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
