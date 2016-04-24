using System;
using System.Linq;
using UnityEngine;
using WavingGrid.Movement;

namespace WavingGrid
{
    public class GridManager : MonoBehaviour
    {
        public NonInteractiveMode NonInteractiveMode = NonInteractiveMode.RandomMovement;

        private GameObject[,] gridPoints;

        public int numRows = 10;
        public int numCols = 10;
        public int SpringNeighbor = 10;
        public int SpringBase = 5;
        public float MaxDisplacement = 8;

        public bool isInteractive = true;
        public GameObject GridPointPrefab;
        private GameObject wavingPoint;
        public float baseDamper = 0.2f;

        // Use this for initialization
        void Start ()
        {
            CreateGrid();

            switch (NonInteractiveMode)
            {
                case NonInteractiveMode.RandomMovement:
                    break;
                case NonInteractiveMode.CircleWaves:
                    wavingPoint = gridPoints[numRows / 2, numCols / 2];
                    wavingPoint.AddComponent<TubeMovement>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

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

                    var cubeGO = parentTransform.gameObject.GetComponentInChildren<Rigidbody>().gameObject;

                    gridPoints[i, j] = cubeGO;

                    parentTransform.parent = transform;
                    parentTransform.localPosition = parentPos;

                    ChangeHeight(parentTransform);
                    AddBaseSpring(cubeGO);

                    switch (NonInteractiveMode)
                    {
                        case NonInteractiveMode.RandomMovement:
                            cubeGO.GetComponent<CubeMovement>().Init();
                            break;
                        case NonInteractiveMode.CircleWaves:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    parentTransform.gameObject
                        .GetComponentInChildren<PressureDetector>()
                        .Init(cubeGO, MaxDisplacement, EnableInteractive);
                }
            }
        }

        private void AddBaseSpring(GameObject cubeGO)
        {
            var joint = cubeGO.AddComponent<SpringJoint>();

            joint.spring = SpringBase;
            joint.damper = baseDamper;

            //joint.enablePreprocessing = false;
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

            //joint.enablePreprocessing = false;

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

            switch (NonInteractiveMode)
            {
                case NonInteractiveMode.RandomMovement:
                    foreach (var cube in gridPoints)
                    {
                        SetGridPointInteractivity(cube, enable);
                    }
                    break;
                case NonInteractiveMode.CircleWaves:
                    SetGridPointInteractivity(wavingPoint, enable);
                    foreach (var gridPoint in gridPoints)
                    {
                        var baseSpringJoint = gridPoint
                            .GetComponents<SpringJoint>()
                            .First(x => x.connectedBody == null);

                        baseSpringJoint.spring = enable ? SpringBase : 0;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetGridPointInteractivity(GameObject cube, bool enable)
        {
            cube.GetComponent<Rigidbody>().isKinematic = !enable;
            cube.GetComponent<MovementBase>().enabled = !enable;
        }
    }
}