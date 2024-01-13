using BehaviorDesigner.Runtime.Tasks;
using MyGame.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommandCondition : Conditional
{
    private EnemyCombatControl _enemyCombatControl;

    public override void OnAwake()
    {
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
    }

    public override TaskStatus OnUpdate()
    {
        return _enemyCombatControl.AttackCommand() ? TaskStatus.Success : TaskStatus.Failure;
    }
}
