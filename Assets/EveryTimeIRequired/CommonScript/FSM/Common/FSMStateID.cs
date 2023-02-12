using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
namespace FSM
{
    public enum FSMStateID
    {
        None,
        Default,
        Dead,
        Idle,
        /// <summary>
        /// 追逐
        /// </summary>
        Pursuit,
        Attacking,
        /// <summary>
        /// 巡逻
        /// </summary>
        Patrolling
    }
}
