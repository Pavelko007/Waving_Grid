using UnityEngine;

namespace WavingGrid
{
    public class GridManager : MonoBehaviour
    {
        private GameObject[,] gridPoints;

        public int numRows = 10;
        public int numCols = 10;
        public int SpringNeighbor = 10;
        public int SpringBase = 5;
        public float MaxDisplacement = 8;

        public bool isInteractive = true;
        public GameObject GridPointPrefab;

        // Use this for initialization
        void Start ()
        {
            CreateGrid();

            AddRowJoints();
            AddColJoints();

            DisableInteractive();
        }

        private void CreateGrid()
        {
            var gridPlane = GameObject.FindGameObjectWithTag("GridPlane");
            
            gridPlane.transform.localScale = new Vector3(numRows, 1, numCols);

            gridPoints = new GameObject[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    const int cubeWidth = 1;

                    var parentPos = new Vector3(i * cubeWidth, 0, j * cubeWidth);

                    var parentTransform = Instantiate(GridPointPrefab).transform;

                    var cubeGO = parentTransform.gameObject
                        .GetComponentInChildren<Rigidbody>()
                        .gameObject;

                    gridPoints[i, j] = cubeGO;

                    parentTransform.parent = transform;
                    parentTransform.localPosition = parentPos;

                    ChangeHeight(parentTransform);
                    AddBaseSpring(cubeGO);

                    cubeGO.GetComponent<CubeMovement>()
                        .Init();

                    //create quad collider for detecting pressure
                    var quad = parentTransform
                        .gameObject
                        .GetComponentInChildren<PressureDetector>()
                        .gameObject;

                    quad.GetComponent<PressureDetector>()
                        .Init(cubeGO, MaxDisplacement, EnableInteractive);
                }
            }
        }

        private void AddBaseSpring(GameObject cubeGO)
        {
            cubeGO.AddComponent<SpringJoint>()
                .spring = SpringBase;
        }

        private void ChangeHeight(Transform parentTransform)
        {
            parentTransform.localScale = new Vector3(1, MaxDisplacement, 1);
        }

        private void AddRowJoints()
        {
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols - 1; j++)
                {
                    LinkCubes(gridPoints[i, j], gridPoints[i, j + 1]);
                }
            }
        }

        private void AddColJoints()
        {
            for (int j = 0; j < numCols; j++)
            {
                for (int i = 0; i < numRows - 1; i++)
                {
                    LinkCubes(gridPoints[i, j], gridPoints[i + 1, j]);
                }
            }
        }

        private void LinkCubes(GameObject first, GameObject second)
        {
            var joint = first.AddComponent<SpringJoint>();
            joint.connectedBody = second.GetComponent<Rigidbody>();
            joint.spring = SpringNeighbor;
        }


        public void DisableInteractive()
        {
            Debug.Log("Disable interactive");
            SetInteractive(false);
        }

        public void EnableInteractive()
        {
            SetInteractive(true);
        }

        private void SetInteractive(bool enable)
        {
            if (isInteractive == enable) return;

            isInteractive = enable;

            foreach (var cube in gridPoints)
            {
                cube.GetComponent<Rigidbody>().isKinematic = !enable;
                cube.GetComponent<CubeMovement>().enabled = !enable;
            }
        }
    }
}