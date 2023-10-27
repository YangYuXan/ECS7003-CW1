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

    private MoveMode moveMode = MoveMode.Normal;

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

                if (Physics.Raycast(ray, out hit))
                {
                    // 计算路径
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(hit.point, path);

                    // 更新LineRenderer来显示路径
                    lineRenderer.positionCount = path.corners.Length;
                    lineRenderer.SetPositions(path.corners);
                }
                break;
        }
    }
}
