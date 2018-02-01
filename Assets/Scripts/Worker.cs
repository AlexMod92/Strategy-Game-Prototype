using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    public enum WorkerType { Stone, Wood };
    public enum WorkerState { MoveToResource, MineResource, MoveToWorkstation, UnloadResource };

    public WorkerType workerType = WorkerType.Wood;
    public WorkerState workerState = WorkerState.MoveToResource;

    public int resourceAmount = 0;

    private Transform resourceDestination = null;
    private Transform workstationDestination = null;

    public string resourceTag = "";
    public string workstationTag = "";

    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        resourceDestination = GameObject.FindGameObjectWithTag(resourceTag).transform;
        workstationDestination = GameObject.FindGameObjectWithTag(workstationTag).transform;
    }

    private void Update()
    {
        switch (workerState)
        {
            case WorkerState.MoveToResource:
                navMeshAgent.isStopped = false;
                MoveToResource(resourceDestination);
                break;
            case WorkerState.MoveToWorkstation:
                navMeshAgent.isStopped = false;
                MoveToWorkstation(workstationDestination);
                break;
        }
    }

    public void ChangeState(WorkerState ws)
    {
        if (workerState != ws)
        {
            workerState = ws;
        }
        else
        {
            return;
        }
    }

    private void MoveToResource(Transform t)
    {
        resourceDestination = GameObject.FindGameObjectWithTag(resourceTag).transform;

        navMeshAgent.destination = t.position;
    }

    private void MoveToWorkstation(Transform t)
    {
        workstationDestination = GameObject.FindGameObjectWithTag(workstationTag).transform.GetChild(0).transform;

        navMeshAgent.destination = t.position;
    }
}