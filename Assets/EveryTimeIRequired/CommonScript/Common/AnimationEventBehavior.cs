using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

///<summary>
///动画事件行为类
///</summary>
namespace Common
{
    public class AnimationEventBehavior : MonoBehaviour
    {
        private Animator anim;
        public event Action attackHandler;
        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        //下面两个方法由动画事件调用
        //动画播放结束
        private void OnCancelAnim(string animParam)
        {
            anim.SetBool(animParam, false);
        }

        //攻击时调用
        private void OnAttack()
        {
            if (attackHandler != null)
            {
                attackHandler();
            }
        }
    }
}

