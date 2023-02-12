using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///状态类
///</summary>
namespace FSM
{
    public abstract class FSMState
    {
        //编号
        public FSMStateID StateID { get; set; }

        //映射表
        private Dictionary<FSMTriggerID, FSMStateID> map;

        private List<FSMTrigger> triggers;

        //要求实现类初始化编号
        public abstract void Init();
        public FSMState()
        {
            map = new Dictionary<FSMTriggerID, FSMStateID>();
            Init();
            triggers = new List<FSMTrigger>();
        }

        public void AddMap(FSMTriggerID triggerID, FSMStateID state)
        {
            //添加映射
            map.Add(triggerID, state);
            //创建条件对象
            CreateTrigger(triggerID);
        }

        private void CreateTrigger(FSMTriggerID triggerID)
        {
            //创建条件对象
            //命名规范：FSM."trigglerID"Triggler
            Type type = Type.GetType("FSM." + triggerID + "Trigger");
            FSMTrigger trigger = Activator.CreateInstance(type) as FSMTrigger;
            triggers.Add(trigger);
        }
        //检测当前状态的条件是否满足
        public void Reason(FSMBase fsm)
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                if (triggers[i].HandleTrigger(fsm))
                {
                    //切换状态
                    fsm.ChangeActiveState(map[triggers[i].TriggerID]);
                    return;
                }
            }
        }

        //为具体状态类添加可选事件
        public virtual void EnterState(FSMBase fsm) { }
        public virtual void ActionState(FSMBase fsm) { }
        public virtual void ExitState(FSMBase fsm) { }


    }
}
