using UnityEngine;

namespace Assets.scripts
{
    public class CubeController : MonoBehaviour
    {
        public float maxDisplacement = 8;
        public float initY;
        public float force = 10;

        private Rigidbody rb;

        // Use this for initialization
        void Start ()
        {
            Init();
        }

        public void Init()
        {
            initY = transform.position.y;
            rb = GetComponent<Rigidbody>();
            GetComponent<Collider>().isTrigger = true;
        }


        // Update is called once per frame
        void Update () {
	
        }

        void OnMouseOver()
        {

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
    }
}
