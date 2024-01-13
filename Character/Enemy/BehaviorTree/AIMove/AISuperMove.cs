using MyGame.Combat;
using MyGame.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGG.Tool;
using BehaviorDesigner.Runtime.Tasks;

public class AISuperMove : Action
{
    private EnemyMovementControl _enemyMovementControl;
    private EnemyCombatControl _enemyCombatControl;

    private Vector3 _moveDirection;
    [SerializeField] private float _moveDistance;
    [SerializeField] private float _time;
    private bool _flag;
    public override void OnAwake()
    {
        _enemyMovementControl = GetComponent<EnemyMovementControl>();
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
        
    }

    public override void OnStart()
    {
        _flag = true;
    }

    public override TaskStatus OnUpdate()
    {
        if (_flag)
        {
            _enemyMovementControl.SuperMoveDirection = SuperMoveState.RIGHTFORWARD;
            _enemyMovementControl.SuperMoveDistance = 5f;
            _enemyMovementControl.AnimationPlay("Dash");            
            _flag = false;
        }
        _time -= Time.deltaTime;
        if(_time< 0)
        {
            _enemyMovementControl.SuperMoveDirection = SuperMoveState.LEFTFORWARD;
            _enemyMovementControl.SuperMoveDistance = 10f;
            _enemyMovementControl.AnimationPlay("Dash");
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    } 
}
