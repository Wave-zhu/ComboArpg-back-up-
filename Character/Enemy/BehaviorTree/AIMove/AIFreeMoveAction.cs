using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;
using MyGame.Movement;
using MyGame.Combat;
using GGG.Tool;

public class AIFreeMoveAction : Action
{
    private EnemyMovementControl _enemyMovementControl;
    private EnemyCombatControl _enemyCombatControl;

    //move set
    private int _lastActionIndex;
    private int _actionIndex;
    private float _actionTimer;


    public override void OnAwake()
    {
        _enemyMovementControl = GetComponent<EnemyMovementControl>();
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
        _lastActionIndex = _actionIndex;
    }
    public override TaskStatus OnUpdate()
    {
        //leave node when need to attack player
        if (!_enemyCombatControl.AttackCommand())
        {
            if (DistanceForTarget() >= 8f)
            {
                _enemyMovementControl.SetAnimatorMovementValue(0f, 1f);
            }
            else if(DistanceForTarget() >= 1.5f)
            {
                FreeMovement();
                ApplyFreeAction();
            }
            else
            {
                _enemyMovementControl.SetAnimatorMovementValue(0f, -0.7f);
            }
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Success;
        }

    }
    private float DistanceForTarget() =>
        DevelopmentToos.DistanceForTarget(EnemyManager.MainInstance.GetMainPlayer(), _enemyMovementControl.transform);

    private void FreeMovement()
    {
        switch(_actionIndex)
        {
            case 0:
                _enemyMovementControl.SetAnimatorMovementValue(-1f, 0f);
                break;
            case 1:
                _enemyMovementControl.SetAnimatorMovementValue(1f, 0f);
                break;
            case 2:
                _enemyMovementControl.SetAnimatorMovementValue(0f, 0f);
                break;
            case 3:
                _enemyMovementControl.SetAnimatorMovementValue(-1f, -1f);
                break;
            case 4:
                _enemyMovementControl.SetAnimatorMovementValue(1f, -1f);
                break;
            case 5:
                _enemyMovementControl.SetAnimatorMovementValue(0f, 1f);
                break;
        }
    }
    private void ApplyFreeAction()
    {
        if (_actionTimer > 0)
        {
            _actionTimer -= Time.deltaTime;
        }
        if (_actionTimer <= 0)
        {
            UpdateActionIndex();
        }

    }

    private void UpdateActionIndex()
    {
        //if same as the lastmove,try once again 
        _lastActionIndex = _actionIndex;
        _actionIndex = Random.Range(0, 6);
        if (_actionIndex ==_lastActionIndex)
        {
           _actionIndex = Random.Range(0, 6);
        }


        if (_actionIndex >= 3)
        {
            _actionTimer = 1f;
        }
        else
        {
            _actionTimer = Random.Range(2f, 5f);
        }
    }
}


