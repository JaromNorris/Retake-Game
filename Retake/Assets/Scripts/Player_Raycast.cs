using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_Raycast : MonoBehaviour
{

    public float viewDistance;
    public GameObject CanvasText;
    private RaycastHit hitObj;

    public GameObject TestSeed;
    public GameObject TestPlant;

    Inventory playerInventory;
    Inventory.Hotbar playerHotbar;

    // Use this for initialization
    void Start()
    {
        playerInventory = GetComponent<Inventory>();
        playerHotbar = playerInventory.hotbar;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug view of the ray
        Debug.DrawRay(this.transform.position, this.transform.forward * viewDistance, Color.blue);

        //Looking at object and left-click
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
        {
            Debug.Log("Selected " + hitObj.collider.name);

            if (hitObj.collider.tag == "Plantable")
            {
                if (!hitObj.collider.gameObject.GetComponent<Plantable_Space>().occupied)
                {
                    GameObject planted = Instantiate(TestPlant, hitObj.transform.position, hitObj.transform.rotation) as GameObject;
                    CanvasText.GetComponent<Text>().text = hitObj.collider.gameObject.GetComponent<Plantable_Space>().onLeftMouseDown(
                        null, planted.GetComponent<Plant>()) + " At " + planted.transform.position;
                }
            }
        }
        //Looking at object and right-click
        else if (Input.GetMouseButtonDown(1) && Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
        {
            Debug.Log("Selected " + hitObj.collider.name);

            if (hitObj.collider.tag == "Plantable")
            {
                if (hitObj.collider.gameObject.GetComponent<Plantable_Space>().occupied)
                {
                    hitObj.collider.gameObject.GetComponent<Plantable_Space>().occupied = false;

                    if (hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentPlant.gameObject)
                    {
                        CanvasText.GetComponent<Text>().text = "Destroyed " +
                            hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentPlant.Species + " At " + 
                            hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentPlant.transform.position;
                        Destroy(hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentPlant.gameObject);
                    }
                    else if (hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentSeed.gameObject)
                    {
                        Destroy(hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentSeed.gameObject);
                    }
                    hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentSeed = null;
                    hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentPlant = null;
                }
            }
            else if (hitObj.collider.tag == "Plant")
            {
                hitObj.collider.gameObject.GetComponent<Plant>().CurrentSpace.occupied = false;
                hitObj.collider.gameObject.GetComponent<Plant>().CurrentSpace.currentPlant = null;
                Destroy(hitObj.collider.gameObject);
            }
            else if (hitObj.collider.tag == "Seed")
            {
                hitObj.collider.gameObject.GetComponent<Seed>().CurrentSpace.occupied = false;
                hitObj.collider.gameObject.GetComponent<Seed>().CurrentSpace.currentPlant = null;
                Destroy(hitObj.collider.gameObject);
            }
        }


    }

}
