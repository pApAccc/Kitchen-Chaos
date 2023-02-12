using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
namespace FSM
{
    public enum FSMTriggerID
    {
        NoHealth,
        SawTarget,
        ReachTarget,
        KilledTarget,
        WithoutAttackRange,
        LoseTarget,
        CompletePartol
    }
}
