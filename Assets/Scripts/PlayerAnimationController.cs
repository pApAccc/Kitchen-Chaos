using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void MoveAnimationControl(bool isPlay)
        {
            animator.SetBool(Animator.StringToHash("IsWalking"), isPlay);
        }
    }
}
