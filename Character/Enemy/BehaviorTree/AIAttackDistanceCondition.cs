using BehaviorDesigner.Runtime.Tasks;
using GGG.Tool;
using MyGame.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackDistanceCondition : Conditional
{
    private EnemyCombatControl _enemyCombatControl;
    [SerializeField] private float _attackDistance;

    public override void OnAwake()
    {
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
    }

    public override TaskStatus OnUpdate()
    {
        return (DevelopmentToos.DistanceForTarget(
            EnemyManager.MainInstance.GetMainPlayer(), _enemyCombatControl.transform) > _attackDistance)
            ? TaskStatus.Success : TaskStatus.Failure;
    }
}
