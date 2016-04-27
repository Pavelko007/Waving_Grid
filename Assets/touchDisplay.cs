using UnityEngine;
using System.Collections;

public class touchDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    Debug.Log(Camera.allCamerasCount);
	}
	
	// Update is called once per frame
	void OnDrawGizmos () {
        foreach (var touch in Input.touches)
        {
            Debug.Log(Input.touchCount);


            Vector3 position = touch.position;
            
            //var screenToWorldPoint = Camera.main.ScreenToWorldPoint(position);
            var spherePos = Camera.main.ScreenPointToRay(position).GetPoint(10);
            
            Gizmos.DrawSphere(spherePos, 1);
        }
    }
}
