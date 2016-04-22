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

                    var parentPos = new Vector3(i * 1, 0, j * 1);

                    var cubeLocalPos = new Vector3(halfCubeWidth, -halfCubeWidth, halfCubeWidth);

                    var quadLocalPos = new Vector3(halfCubeWidth, 0, halfCubeWidth);

                    var parentTransform = new GameObject("Scaler object").transform;

                    parentTransform.parent = transform;
                    parentTransform.localPosition = parentPos;

                    var cubeGO = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cubesGrid[i, j] = cubeGO;
                    cubeGO.GetComponent<Renderer>().material = cubeMat;
                    cubeGO.transform.parent = parentTransform;
                    cubeGO.transform.localPosition = cubeLocalPos;

                    AddRigidBody(cubeGO);

                    cubeGO.GetComponent<Collider>().enabled = false;
                    parentTransform.localScale = new Vector3(1, MaxDisplacement, 1);

                    cubeGO.AddComponent<SpringJoint>().spring = SpringBase;
                    cubeGO.AddComponent<CubeMovement>();

                    //create quad collider for detecting pressure
                    var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    
                    var pressureDetector = quad.AddComponent<PressureDetector>();

                    pressureDetector.Init(cubeGO, MaxDisplacement);
                    cubeGO.GetComponent<CubeMovement>().initialY = pressureDetector.initY;
                    quad.transform.parent = cubeGO.transform.parent;
                    quad.transform.localPosition = quadLocalPos;
                    quad.transform.Rotate(90,0,0);
                    quad.GetComponent<Renderer>().enabled = false;
                }
            }
        }

        private void AddRigidBody(GameObject cubeGO)
        {
            var rb = cubeGO.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
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