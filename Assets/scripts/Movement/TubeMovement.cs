using UnityEngine;

namespace WavingGrid.Movement
{
    public class TubeMovement : MovementBase
    {
        public float amlitude = 4;

        public float speed = 4;

        private bool isMovingUp;

        void Awake ()
        {
            InitialY = transform.position.y;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isMovingUp)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
                if (transform.position.y > InitialY + amlitude) isMovingUp = false;
            }
            else
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
                if (transform.position.y < InitialY - amlitude) isMovingUp = true;
            }
        }
    }
}

