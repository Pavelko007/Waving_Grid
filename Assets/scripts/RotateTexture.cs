using UnityEngine;

namespace WavingGrid
{
    public class RotateTexture : MonoBehaviour
    {
        public Material material;
        public int degreesDeviation = 15;
        public float rotationSpeed = 10;

        // Use this for initialization
        void Start ()
        {
            material.SetFloat("_RotationDegrees", 0);
        }
	
        // Update is called once per frame
        void Update ()
        {
            float curAngle = material.GetFloat("_RotationDegrees") * Mathf.Rad2Deg;

            Debug.Log("cur angle " + curAngle);

            if (Mathf.Abs(curAngle) > degreesDeviation)
            {
                rotationSpeed = -rotationSpeed;
            }

            var newAngle = curAngle + Time.deltaTime * rotationSpeed;

            material.SetFloat("_RotationDegrees", newAngle * Mathf.Deg2Rad);
        }
    }
}
