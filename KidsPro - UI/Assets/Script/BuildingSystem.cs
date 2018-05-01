using UnityEngine;
using System.Collections;

public class BuildingSystem : MonoBehaviour
{
    private BlockSystem bSys;
    private Vector3 buildPos;
    private GameObject currentTempBlock;
    [SerializeField]
    private GameObject blockPrefab;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit buildPosHit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 120)), out buildPosHit, 10))
        {
        Vector3 point = buildPosHit.point;
            buildPos = new Vector3(Mathf.Round(point.x), Mathf.Round(point.y), Mathf.Round(point.z));
        }

        currentTempBlock.transform.position = buildPos;
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBlock();
        }
    }

    private void PlaceBlock()
    {
        GameObject newBlock = Instantiate(blockPrefab, buildPos, Quaternion.identity);
        Block tempBlock = bSys.allBlocks[0];
        newBlock.name = tempBlock.blockName;
        newBlock.GetComponent<MeshRenderer>().material = tempBlock.blockMaterial;
    }
}
