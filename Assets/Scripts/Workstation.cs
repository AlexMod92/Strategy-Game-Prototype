using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Workstation : MonoBehaviour
{
    [Header("Resources")]
    public int localResourceAmount = 0;
    [Space]
    public int buildedResourceAmount = 0;
    public int buildingResourceCost = 20;
    [Space]
    public float resourceUnloadTime = 2.0f;

    [Header("UI")]
    public GameObject canvas = null;
    [Space]
    public Text localResourceAmountText = null;
    public Text buildedResourceAmountText = null;
    [Space]
    public Button buildButton = null;

    [Header("Tag")]
    public string buildingTag = "";

    private Ray ray;
    private RaycastHit raycastHit;

    private readonly string workerTag = "Worker";

    private void Update()
    {
        SelectBuilding();
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == workerTag)
        {
            other.gameObject.GetComponent<Worker>().ChangeState(Worker.WorkerState.UnloadResource);
            other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;

            StartCoroutine(UnloadResource(other.gameObject));
        }
    }

    private IEnumerator UnloadResource(GameObject worker)
    {
        yield return new WaitForSeconds(resourceUnloadTime);

        localResourceAmount += worker.GetComponent<Worker>().resourceAmount;
        worker.GetComponent<Worker>().resourceAmount = 0;

        worker.GetComponent<Worker>().ChangeState(Worker.WorkerState.MoveToResource);
        worker.GetComponent<NavMeshAgent>().isStopped = false;
    }

    private void SelectBuilding()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out raycastHit, 200.0f))
            {
                if (raycastHit.collider.gameObject.tag == buildingTag)
                {
                    canvas.SetActive(true);
                }
            }
        }
    }

    public void BuildFromResource()
    {
        if (localResourceAmount >= buildingResourceCost)
        {
            StartCoroutine(BuildNewResource());
        }
    }

    private IEnumerator BuildNewResource()
    {
        yield return new WaitForSeconds(2.0f);

        localResourceAmount -= buildingResourceCost;

        buildedResourceAmount++;
    }

    private void UpdateUI()
    {
        localResourceAmountText.text = localResourceAmount.ToString();
        buildedResourceAmountText.text = buildedResourceAmount.ToString();

        if (localResourceAmount < buildingResourceCost)
        {
            buildButton.interactable = false;
        }
        else
        {
            buildButton.interactable = true;
        }
    }
}