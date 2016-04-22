using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts
{
    //[ExecuteInEditMode]
    public class GridManager : MonoBehaviour
    {
        private GameObject[,] cubesGrid;

        public int numRows = 10;
        public int numCols = 10;
        public Material cubeMat;
        public int SpringNeighbor = 10;
        public int SpringBase = 5;

        // Use this for initialization
        void Start ()
        {
            cubesGrid = new GameObject[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    var cubePos = new Vector3(i * 1, 0, j * 1);
                    cubesGrid[i,j] = CreateCube(cubePos);
                }
            }

            AddRowJoints();
            AddColJoints();
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

        private GameObject CreateCube(Vector3 position)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            cube.GetComponent<Renderer>().material = cubeMat;
            var cubeTransform = cube.transform;
            cubeTransform.position = position;

            cubeTransform.parent = transform;
            var rb = cube.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            var cubeController = cube.AddComponent<CubeController>();

            cubeController.Init();
            cubeTransform.localScale = new Vector3(1, cubeController.maxDisplacement, 1);

            cube.AddComponent<SpringJoint>().spring = SpringBase;


            return cube;
        }


        // Update is called once per frame
        void Update () {
	
        }
    }
}
