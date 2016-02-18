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
    //Inventory.Hotbar playerHotbar;

    // Use this for initialization
    void Start()
    {
        playerInventory = GetComponent<Inventory>();
        //playerHotbar = playerInventory.hotbar;
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
                            hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentPlant.species + " At " + 
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
                hitObj.collider.gameObject.GetComponent<Plant>().currentSpace.occupied = false;
                hitObj.collider.gameObject.GetComponent<Plant>().currentSpace.currentPlant = null;
                Destroy(hitObj.collider.gameObject);
            }
            else if (hitObj.collider.tag == "Seed")
            {
                hitObj.collider.gameObject.GetComponent<Seed>().currentSpace.occupied = false;
                hitObj.collider.gameObject.GetComponent<Seed>().currentSpace.currentPlant = null;
                Destroy(hitObj.collider.gameObject);
            }
        }

        // I am hijacking this space so we can have all of our user inputs in one place
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerInventory.currentIndex = 0;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && playerInventory.hotbarSize >= 2)
        {
            playerInventory.currentIndex = 1;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && playerInventory.hotbarSize >= 3)
        {
            playerInventory.currentIndex = 2;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 3";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && playerInventory.hotbarSize >= 4)
        {
            playerInventory.currentIndex = 3;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 4";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && playerInventory.hotbarSize >= 5)
        {
            playerInventory.currentIndex = 4;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 5";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && playerInventory.hotbarSize >= 6)
        {
            playerInventory.currentIndex = 5;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 6";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && playerInventory.hotbarSize >= 7)
        {
            playerInventory.currentIndex = 6;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 7";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && playerInventory.hotbarSize >= 8)
        {
            playerInventory.currentIndex = 7;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 8";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && playerInventory.hotbarSize >= 9)
        {
            playerInventory.currentIndex = 8;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 9";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) && playerInventory.hotbarSize >= 10)
        {
            playerInventory.currentIndex = 9;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " " + playerInventory.currentItem.Type;
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 10";
        }

    }

}
