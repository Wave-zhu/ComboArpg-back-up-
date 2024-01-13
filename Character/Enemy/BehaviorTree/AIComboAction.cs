using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Combat;

public class AIComboAction : Action
{
    private EnemyCombatControl _enemyCombatControl;

    public override void OnAwake()
    {
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_enemyCombatControl.AttackCommand())
        {
            _enemyCombatControl.AIBasicAttackInput();
            return TaskStatus.Running;
        }
        return TaskStatus.Success;
    }
}
