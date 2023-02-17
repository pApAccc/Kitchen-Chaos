using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class CuttingCounterAnimationController : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] private CuttingCounter cuttingCounter;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            cuttingCounter.OnCut += (object sender, EventArgs e) =>
            {
                animator.SetTrigger(Animator.StringToHash("Cut"));
            };
        }

    }
}
