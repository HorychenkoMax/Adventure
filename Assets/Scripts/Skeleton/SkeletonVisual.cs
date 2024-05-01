using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAi;
    [SerializeField] private EnemyEntity _enemyEntity;

    private Animator _animator;

    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIP0LIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _enemyAi.OnEnemyAttack += _enemyAi_OnEnemyAttack;
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAi.IsRunning());
        _animator.SetFloat(CHASING_SPEED_MULTIP0LIER, _enemyAi.GetRoamingAnimationSpeed());
    }
    private void OnDestroy()
    {
        _enemyAi.OnEnemyAttack -= _enemyAi_OnEnemyAttack;
    }

    public void PolygonColliderTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }

    public void PolygonColliderTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }

    private void _enemyAi_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
}
