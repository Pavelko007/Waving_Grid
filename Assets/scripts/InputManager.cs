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
                bool wasHitAny = false;

                foreach (var touch in Input.touches)
                {
                    Debug.Log(Input.touchCount);

                    if (TryHitDetector(touch.position)) wasHitAny = true;
                }

                if(!wasHitAny) GridManager.DisableInteractive();
            }
            else
            {
                var wasHit = TryHitDetector(Input.mousePosition);

                if (!wasHit) GridManager.DisableInteractive();
            }
        }

        private bool TryHitDetector(Vector3 screenPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit hit;

            bool wasHit = Physics.Raycast(ray, out hit, 100);

            if (wasHit)
            {
                Debug.DrawLine(ray.origin, hit.point);

                hit.collider.GetComponent<PressureDetector>().isHovering = true;
            }
            return wasHit;
        }
    }
}
