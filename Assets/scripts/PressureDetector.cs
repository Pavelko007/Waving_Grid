using System;
using UnityEngine;

namespace WavingGrid
{
    public class PressureDetector : MonoBehaviour {

        public float maxDisplacement = 8;
        public float initY;
        public float force = 10;

        private Rigidbody rb;
        public Action OnMouseOverAction;

        void OnMouseOver()
        {
            OnMouseOverAction();

            if (transform.position.y < initY + maxDisplacement)
            {
                rb.velocity = force * Vector3.up;
            }
            else
            {
                rb.isKinematic = true;
            }
        }

        void OnMouseExit()
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }

        internal void Init(GameObject cube, float maxDisplacement)
        {
            initY = cube.GetComponent<CubeMovement>().InitialY;

            rb = cube.GetComponent<Rigidbody>();

            this.maxDisplacement = maxDisplacement;
        }
    }
}