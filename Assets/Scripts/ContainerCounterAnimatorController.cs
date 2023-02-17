using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class ContainerCounterAnimatorController : MonoBehaviour
    {
        [SerializeField] private ContainerCounter containerCounter;
        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            containerCounter.OnInteract += (object sender, EventArgs e) =>
            {
                animator.SetTrigger(Animator.StringToHash("OpenClose"));
            };
        }
    }
}
