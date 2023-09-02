using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DA{
    public class AnimationHandler : MonoBehaviour
    {
        private Animator anim;
        int vertical;
        int horizontal;
        public bool canRotate;

        public void Start()
        {
            anim= GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            vertical = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(){

        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting",isInteracting);
            anim.CrossFade(targetAnim,0.2f);
        }

        public void OnAnimatorMove()
        {

        }

        public void PlayerDeathAnimation()
        {
            
        }
    }
}
