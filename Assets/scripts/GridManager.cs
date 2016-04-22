using UnityEngine;

namespace WavingGrid
{
    public class GridManager : MonoBehaviour
    {
        private GameObject[,] cubesGrid;

        public int numRows = 10;
        public int numCols = 10;
        public Material cubeMat;
        public int SpringNeighbor = 10;
        public int SpringBase = 5;
        public float MaxDisplacement = 8;

        public bool isInteractive = true;

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

            cubesGrid = new GameObject[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    const float halfCubeWidth = .5f;

                    Vector3 cubePos = new Vector3(i * 1, 0, j * 1);
                    cubePos += new Vector3(halfCubeWidth, 0, halfCubeWidth);

                    Vector3 position = cubePos;

                    GameObject cubeGO = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cubeGO.GetComponent<Renderer>().material = cubeMat;
                    var cubeTransform = cubeGO.transform;

                    //move down by half cube height for scaling from one side
                    position += new Vector3(0, -halfCubeWidth, 0);

                    cubeTransform.position = position;

                    GameObject go = new GameObject("Scaler object");
                    go.transform.parent = transform;
                    cubeGO.transform.parent = go.transform;


                    var rb = cubeGO.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;

                    cubeGO.GetComponent<Collider>().enabled = false;
                    go.transform.localScale = new Vector3(1, MaxDisplacement, 1);

                    cubeGO.AddComponent<SpringJoint>().spring = SpringBase;

                    cubeGO.AddComponent<CubeMovement>();

                    cubesGrid[i, j] = cubeGO;

                    //create quad collider for detecting pressure
                    var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    
                    PressureDetector pressureDetector = quad.AddComponent<PressureDetector>();
                    pressureDetector.Init(cubeGO, MaxDisplacement);
                    cubeGO.GetComponent<CubeMovement>().initialY = pressureDetector.initY;

                    quad.transform.position = cubePos;
                    quad.transform.Rotate(90,0,0);
                    quad.transform.parent = cubeGO.transform.parent;
                    quad.GetComponent<Renderer>().enabled = false;
                }
            }
        }

        private void AddRowJoints()
        {
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols - 1; j++)
                {
                    LinkCubes(cubesGrid[i, j], cubesGrid[i, j + 1]);
                }
            }
        }

        private void AddColJoints()
        {
            for (int j = 0; j < numCols; j++)
            {
                for (int i = 0; i < numRows - 1; i++)
                {
                    LinkCubes(cubesGrid[i, j], cubesGrid[i + 1, j]);
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

            foreach (var cube in cubesGrid)
            {
                cube.GetComponent<Rigidbody>().isKinematic = !enable;
                cube.GetComponent<CubeMovement>().enabled = !enable;
            }
        }
    }
}