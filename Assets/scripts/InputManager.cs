using UnityEngine;

namespace WavingGrid
{
    public class InputManager : MonoBehaviour
    {
        public GridManager GridManager;

        void Update()
        {
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
