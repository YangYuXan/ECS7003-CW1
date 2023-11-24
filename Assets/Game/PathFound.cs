using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFound : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public NavMeshAgent agent;

    public enum MoveMode
    {
        TurnMode,
        Normal
    }

    public MoveMode moveMode = MoveMode.Normal;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (moveMode)
        {
            case MoveMode.Normal:
                break;

            case MoveMode.TurnMode:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                List<Vector3> road = new();

                if (Physics.Raycast(ray, out hit))
                {
                    // 计算路径
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(hit.point, path);

                    // 更新LineRenderer来显示路径
                    //lineRenderer.positionCount = path.corners.Length;
                    //lineRenderer.SetPositions(path.corners);

                    
                    for (int i = 0; i < path.corners.Length; i++)
                    {
                        road.Add(new Vector3(path.corners[i].x, path.corners[i].y+.8f, path.corners[i].z));
                    }

                    lineRenderer.positionCount = road.Count;
                    lineRenderer.SetPositions(road.ToArray());

                }
                break;
        }
    }
}
