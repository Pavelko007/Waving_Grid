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

        private bool isOver = false;

        void OnMouseEnter()
        {
            isOver = true;
            OnMouseEnterAction();
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

        internal void Init(GameObject gridPoint, float maxDisplacement, Action onMouseEnterAction)
        {
            OnMouseEnterAction = onMouseEnterAction;
            initY = gridPoint.transform.position.y;

            rb = gridPoint.GetComponent<Rigidbody>();

            MaxDisplacement = maxDisplacement;
        }
    }
}