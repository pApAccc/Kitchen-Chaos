
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

///<summary>
///状态机
///</summary>
namespace FSM
{
    public class FSMBase : MonoBehaviour
    {
        private List<FSMState> States;

        private FSMState currentState;

        [Tooltip("默认状态ID")]
        public FSMStateID defaultStateID;
        //默认状态
        FSMState defaultState;

        public string fileName = "AIConfig.txt";
        private void Start()
        {
            // InitComponent();
            ConfigFSM();
            InitDefaultState();

        }

        //配置状态机
        //private void ConfigFSM()
        //{
        //    States = new List<FSMState>();
        //    //创建状态对象
        //    IdleState idle = new IdleState();
        //    idle.AddMap(FSMTriggerID.NoHealth, FSMStateID.Dead);
        //    States.Add(idle);
        //    DeadState dead = new DeadState();
        //    States.Add(dead);

        //}
        private void ConfigFSM()
        {
            States = new List<FSMState>();
            var map = AIConfigurationReaderFactory.GetMap(fileName);

            //把状态加入States列表
            foreach (var item in map.Keys)
            {
                Type type = Type.GetType("FSM." + item + "State");
                FSMState state = Activator.CreateInstance(type) as FSMState;
                States.Add(state);

                foreach (var val in map[item].Keys)
                {
                    state.AddMap((FSMTriggerID)Enum.Parse(typeof(FSMTriggerID), val), (FSMStateID)Enum.Parse(typeof(FSMStateID), map[item][val]));
                }
            }
        }
        private void InitDefaultState()
        {
            //查找默认状态
            defaultState = States.Find(s => s.StateID == defaultStateID);
            currentState = defaultState;
            //进入状态
            currentState.EnterState(this);
        }

        //每帧处理的逻辑
        public void Update()
        {
            //判断条件是否满足
            currentState.Reason(this);
            //执行当前状态逻辑
            currentState.ActionState(this);
        }

        //切换状态
        public void ChangeActiveState(FSMStateID stateID)
        {
            //离开上一个状态
            currentState.ExitState(this);
            //如果状态时默认状态 就赋值默认状态
            currentState = stateID == FSMStateID.Default ? defaultState : States.Find(s => s.StateID == stateID);
            //进入下一个状态
            currentState.EnterState(this);
        }
        //[HideInInspector]
        //public Animator anim;


    }
}
