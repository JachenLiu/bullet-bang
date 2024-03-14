using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace BulletBang
{
    public class PlayerCamera : NetworkBehaviour
    {
        public Transform Target;
        public Camera playerCamera;


        private void Awake()
        {
            playerCamera = GetComponent<Camera>();
        }
        void Start()
        {
            if (playerCamera.enabled)
            {
                playerCamera.transform.parent = null;
            }
        }


        public float MouseSensitivity = 10f;

        private float verticalRotation;
        private float horizontalRotation;

        void LateUpdate()
        {
            if(Target == null)
            {
                return;
            }
            if (!playerCamera.enabled)
            {
                return;
            }

            playerCamera.transform.position = Target.position;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            verticalRotation -= mouseY * MouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

            horizontalRotation += mouseX * MouseSensitivity;

            transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        }

    }
}
