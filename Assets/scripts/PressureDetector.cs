using System;
using UnityEngine;

namespace WavingGrid
{
    public class PressureDetector : MonoBehaviour {

        public float MaxDisplacement = 8;
        public float initY;
        public float speed = 2;

        private Rigidbody rb;
        public Action OnMouseEnterAction;

        public bool isHovering = false;
        private bool wasHoveringLastFrame = false;

        private void BeginPress()
        {
            rb.isKinematic = true;
        }

        void Update()
        {
            if (isHovering)
            {
                if (!wasHoveringLastFrame)
                {
                    OnMouseEnterAction();
                    BeginPress();
                }

                Press();

                wasHoveringLastFrame = isHovering;
            }
            else if (wasHoveringLastFrame) EndPress();

            wasHoveringLastFrame = isHovering;
            isHovering = false;
        }


        private void Press()
        {
            if (rb.transform.position.y < initY + MaxDisplacement)
            {
                rb.transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
        }

        private void EndPress()
        {
            rb.isKinematic = false;
        }

        internal void Init(GameObject gridPoint, float maxDisplacement, Action onMouseEnterAction)
        {
            OnMouseEnterAction = onMouseEnterAction;
            initY = gridPoint.transform.position.y;

            rb = gridPoint.GetComponent<Rigidbody>();

            MaxDisplacement = maxDisplacement;
        }
    }
}