using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour
{
    public float initialY ;
    public float amlitude = 5;
    public float speed = 1;
    private bool isMovingUp;

    void OnEnable()
    {
        isMovingUp = Random.Range(0, 1) < 0.5;
    }

	// Update is called once per frame
	void Update () {
	    //if (isMovingUp)
	    //{
	    //    transform.position += Vector3.up * speed * Time.deltaTime;
	    //}
	}
}
