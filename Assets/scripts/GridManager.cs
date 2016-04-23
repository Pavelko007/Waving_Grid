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
        public GameObject CubePrefab;

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

                    var parentPos = new Vector3(i * 1, 0, j * 1);
                    var parentTransform = new GameObject("Scaler object").transform;

                    var cubeLocalPos = new Vector3(cubeWidth / 2f, -cubeWidth / 2f, cubeWidth / 2f);
                    var cubeGO = Instantiate(CubePrefab, cubeLocalPos, Quaternion.identity) as GameObject;
                    gridPoints[i, j] = cubeGO;

                    cubeGO.transform.parent = parentTransform;
                    parentTransform.parent = transform;
                    parentTransform.localPosition = parentPos;

                    ChangeHeight(parentTransform);
                    AddBaseSpring(cubeGO);

                    cubeGO.GetComponent<CubeMovement>()
                        .Init();

                    //create quad collider for detecting pressure
                    var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.parent = parentTransform;
                    var quadLocalPos = new Vector3(cubeWidth / 2f, 0, cubeWidth / 2f);
                    quad.transform.localPosition = quadLocalPos;

                    quad.AddComponent<PressureDetector>()
                        .Init(cubeGO, MaxDisplacement, EnableInteractive);

                    quad.transform.Rotate(90,0,0);
                    quad.GetComponent<Renderer>()
                        .enabled = false;
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