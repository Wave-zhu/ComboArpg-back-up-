using BehaviorDesigner.Runtime.Tasks;
using GGG.Tool;
using MyGame.Combat;
using MyGame.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIUseKick : Action
{
    private EnemyCombatControl _enemyCombatControl;

    public override void OnAwake()
    {
        _enemyCombatControl = GetComponent<EnemyCombatControl>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Random.Range(0, 3)==1)
        {
            _enemyCombatControl.SwitchAttackStyle();
        }
        return TaskStatus.Success;
    }
}
