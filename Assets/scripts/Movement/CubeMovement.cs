using UnityEngine;

namespace WavingGrid.Movement
{
    public class CubeMovement : MovementBase
    {
        public float amlitude = 2;
        public float speed = 1;
        private bool isMovingUp;
        private float minSpeed = .3f;
        private float maxSpeed = 1.5f;

        void OnEnable()
        {
            ChangeSpeedAndDirection();
        }

        public void Init()
        {
            InitialY = gameObject.transform.position.y;
            ChangeSpeedAndDirection();
        }

        private void ChangeSpeedAndDirection()
        {
            isMovingUp = Random.Range(0, 1.0f) < 0.5;

            speed = Random.Range(minSpeed, maxSpeed);
        }

        // Update is called once per frame
        void FixedUpdate () {

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
