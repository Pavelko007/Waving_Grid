using System;
using UnityEngine;

namespace WavingGrid
{
    public class PressureDetector : MonoBehaviour {

        public float MaxDisplacement = 8;
        public float initY;
        public float speed = 2;

        private Rigidbody rb;
        public Action OnMouseOverAction;

        private bool isOver = false;

        void OnMouseEnter()
        {
            isOver = true;
            OnMouseOverAction();
            rb.isKinematic = true;
        }

        void FixedUpdate()
        {
            if (!isOver) return;

            if (rb.transform.position.y < initY + MaxDisplacement)
            {
                rb.transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            
        }

        void OnMouseExit()
        {
            isOver = false;
            rb.isKinematic = false;
        }

        internal void Init(GameObject cube, float maxDisplacement, Action onMouseOverAction)
        {
            OnMouseOverAction = onMouseOverAction;
            initY = cube.GetComponent<CubeMovement>().InitialY;

            rb = cube.GetComponent<Rigidbody>();

            MaxDisplacement = maxDisplacement;
        }
    }
}