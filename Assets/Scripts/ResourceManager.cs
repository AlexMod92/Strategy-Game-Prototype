using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    [Header("Resources")]
    public int resourceGold = 50;
    public int resourceWood = 0;
    public int resourceStone = 0;
    [Space]
    public int workerCost = 10;

    [Header("Worker")]
    public GameObject workerStone = null;
    public GameObject workerWood = null;
    [Space]
    public Transform workerSpawnPosition = null;

    [Header("UI")]
    public GameObject canvas = null;
    [Space]
    public Text resourceGoldText = null;
    public Text resourceWoodText = null;
    public Text resourceStoneText = null;
    [Space]
    public Button workerWoodButton = null;
    public Button workerStoneButton = null;

    [Header("Workstations")]
    public GameObject woodWorkstation = null;
    public GameObject stoneWorkstation = null;

    private Ray ray;
    private RaycastHit raycastHit;

    private readonly string buildingTag = "Town Hall";

    private void Start()
    {
        resourceWood = woodWorkstation.GetComponent<Workstation>().localResourceAmount;
        resourceWood = stoneWorkstation.GetComponent<Workstation>().localResourceAmount;
    }

    private void Update()
    {
        SelectBuilding();
        UpdateUI();
    }

    private void UpdateUI()
    {
        resourceGoldText.text = resourceGold.ToString();
        resourceWoodText.text = resourceWood.ToString();
        resourceStoneText.text = resourceStone.ToString();

        if (resourceGold < workerCost)
        {
            workerWoodButton.interactable = false;
            workerStoneButton.interactable = false;
        }
        else
        {
            workerWoodButton.interactable = true;
            workerStoneButton.interactable = true;
        }
    }

    public void SpawnWorker(GameObject worker)
    {
        if (resourceGold >= workerCost)
        {
            resourceGold -= workerCost;

            Instantiate(worker, workerSpawnPosition.position, worker.transform.rotation);
        }
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
}