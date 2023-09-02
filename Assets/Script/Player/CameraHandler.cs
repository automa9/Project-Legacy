using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace DA
{
    public class CameraHandler : MonoBehaviour
    {
        PhotonView view;
        private  Transform targetTransform;
        public Transform cameraTransform; 
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        private void Awake(){
        targetTransform = GameObject.FindWithTag("Player").transform;
        view = GetComponent<PhotonView>();
           if ( view == null || view.IsMine ){
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraPivotTransform.localPosition.z;
             // ignoreLayers = 
           }
        }

        public void FollowTarget(float delta){

            Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta/followSpeed);
            myTransform.position = targetPosition;

        }

    }
}


