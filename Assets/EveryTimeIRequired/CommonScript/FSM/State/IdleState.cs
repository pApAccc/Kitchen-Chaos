using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

///<summary>
///默认状态
///</summary>
namespace FSM

{
    public class IdleState : FSMState
    {
        public override void Init()
        {
            StateID = FSMStateID.Idle;
        }
        public override void ActionState(FSMBase fsm)
        {
            base.EnterState(fsm);
            Debug.Log("Idle");
            //播放动画
        }
    }
}
