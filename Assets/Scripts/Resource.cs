using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Resource : MonoBehaviour
{
    public int localResourceAmount = 5000;
    public int mineableResourceAmount = 50;
    [Space]
    public float resourceMiningTime = 2.0f;

    private readonly string workerTag = "Worker";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == workerTag)
        {
            if (other.gameObject.GetComponent<Worker>().workerState != Worker.WorkerState.MoveToWorkstation)
            {
                other.gameObject.GetComponent<Worker>().ChangeState(Worker.WorkerState.MineResource);
                other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;

                StartCoroutine(MineResource(other.gameObject));
            }
        }
    }

    private IEnumerator MineResource(GameObject worker)
    {
        yield return new WaitForSeconds(resourceMiningTime);

        localResourceAmount -= mineableResourceAmount;
        worker.GetComponent<Worker>().resourceAmount += mineableResourceAmount;

        worker.GetComponent<Worker>().ChangeState(Worker.WorkerState.MoveToWorkstation);
        worker.GetComponent<NavMeshAgent>().isStopped = false;
    }
    

    private void Update()
    {
        if (localResourceAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
}