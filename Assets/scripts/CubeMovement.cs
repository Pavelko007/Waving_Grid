using UnityEngine;

namespace WavingGrid
{
    public class CubeMovement : MonoBehaviour
    {
        public float initialY ;
        public float amlitude = 2;
        public float speed = 1;
        private bool isMovingUp;
        private float minSpeed = .3f;
        private float maxSpeed = 1.5f;

        void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            isMovingUp = Random.Range(0, 1.0f) < 0.5;

            speed = Random.Range(minSpeed, maxSpeed);
        }

        // Update is called once per frame
        void Update () {

            if (isMovingUp)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
                if (transform.position.y > initialY + amlitude) isMovingUp = false;
            }
            else
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
                if (transform.position.y < initialY - amlitude) isMovingUp = true;
            }
        }
    }
}
