using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///条件类
///</summary>
namespace FSM
{
    public abstract class FSMTrigger
    {
        //编号
        public FSMTriggerID TriggerID { get; set; }

        public FSMTrigger()
        {
            Init();
        }

        //要求子类必须初始化
        public abstract void Init();

        //具体逻辑处理
        public abstract bool HandleTrigger(FSMBase fsm);

    }
}
