using System.Collections.Generic;
using UnityEngine;

namespace WavingGrid
{
    public class InputManager : MonoBehaviour
    {
        public GridManager GridManager;
        private Dictionary<int, PressureDetector> activeDetectors = new Dictionary<int, PressureDetector>();
         
        void Update()
        {
            if (Input.touchSupported)
            {
                foreach (var touch in Input.touches)
                {
                    PressureDetector detector;
                    switch (touch.phase)
                    {
                        case TouchPhase.Ended:
                            detector = activeDetectors[touch.fingerId];
                            detector.SendMessage("OnMouseExit");
                            activeDetectors.Remove(touch.fingerId);
                            break;
                        case TouchPhase.Began:
                            var ray = Camera.main.ScreenPointToRay(touch.position);

                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit, 100))
                            {
                                detector = hit.collider.GetComponent<PressureDetector>();

                                hit.collider.SendMessage("OnMouseEnter");
                                activeDetectors.Add(touch.fingerId, detector);
                            }
                            break;
                    }
                    
                }

                if (activeDetectors.Count == 0)
                {
                    if(GridManager.isInteractive) GridManager.DisableInteractive();
                }


                //GridManager.DisableInteractive();
            }
            else
            {
                if (!GridManager.isInteractive) return;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.DrawLine(ray.origin, hit.point);
                }
                else
                {
                    GridManager.DisableInteractive();
                }
            }
            
        }
    }
}
