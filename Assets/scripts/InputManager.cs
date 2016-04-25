using UnityEngine;

namespace WavingGrid
{
    public class InputManager : MonoBehaviour
    {
        public GridManager GridManager;

        void Update()
        {
            if (Input.touchSupported)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                    {
                        GridManager.DisableInteractive();
                    }
                }
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
