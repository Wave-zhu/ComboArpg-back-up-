using BehaviorDesigner.Runtime.Tasks;
using GGG.Tool;
using MyGame.Combat;
using MyGame.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveForward : Action
{
    private EnemyMovementControl _enemyMovementControl;

    public override void OnAwake()
    {
        _enemyMovementControl = GetComponent<EnemyMovementControl>();
    }

    public override TaskStatus OnUpdate()
    {
        if(DevelopmentToos.DistanceForTarget(
            EnemyManager.MainInstance.GetMainPlayer(), _enemyMovementControl.transform) > 3f)
        {
            _enemyMovementControl.SetApplyMovement(true);
            _enemyMovementControl.SetAnimatorMovementValue(0f, 1f);
            return TaskStatus.Running;
        }
        return TaskStatus.Success;
    }
}
