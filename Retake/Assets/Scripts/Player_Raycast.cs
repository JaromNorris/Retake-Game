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
        if (Input.GetMouseButtonDown(0) && playerInventory.currentItem != null && Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
        {
            Debug.Log("Selected " + hitObj.collider.name);

            if (hitObj.collider.tag == "Plantable")
            {
                if (!hitObj.collider.gameObject.GetComponent<Plantable_Space>().occupied)
                {
                    GameObject planted;
                    if (playerInventory.currentItem.type.Equals("Plant"))
                    {
                        planted = Instantiate(TestPlant, hitObj.transform.position, hitObj.transform.rotation) as GameObject;
                        hitObj.collider.GetComponent<Plantable_Space>().occupied = true;
                        hitObj.collider.GetComponent<Plantable_Space>().currentPlant = planted.GetComponent<Plant>();
						planted.GetComponent<PlantableObject>().currentSpace = hitObj.collider.gameObject.GetComponent<Plantable_Space>();
                        playerInventory.Remove(playerInventory.currentIndex);
                    }
                    else
                    {
                        planted = Instantiate(TestSeed, hitObj.transform.position, hitObj.transform.rotation) as GameObject;
                        hitObj.collider.GetComponent<Plantable_Space>().occupied = true;
						hitObj.collider.GetComponent<Plantable_Space>().currentSeed = planted.GetComponent<Seed>();
						planted.GetComponent<PlantableObject>().currentSpace = hitObj.collider.gameObject.GetComponent<Plantable_Space>();
						playerInventory.Remove(playerInventory.currentIndex);
                    }
                    if(playerInventory.currentItem != null)
                        CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                            + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
                    else
                        CanvasText.GetComponent<Text>().text = "No item in slot " + (playerInventory.currentIndex + 1);
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

                    if (hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentPlant != null)
                    {
                        Plant currentPlant = hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentPlant;
                        playerInventory.Add(currentPlant);
                        if(playerInventory.currentItem != null && currentPlant.Type.Equals(playerInventory.currentItem.type))
                            CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                                + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
                        Destroy(currentPlant.gameObject);
                    }
                    else if (hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentSeed != null)
                    {
                        Seed currentSeed = hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentSeed;
                        playerInventory.Add(currentSeed);
                        if (playerInventory.currentItem != null && currentSeed.Type.Equals(playerInventory.currentItem.type))
                            CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                                + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
                        Destroy(currentSeed.gameObject);
                    }
                    hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentSeed = null;
                    hitObj.collider.gameObject.GetComponent<Plantable_Space>().currentPlant = null;
                }
            }
            else if (hitObj.collider.tag == "Plant")
            {
                Plant currentPlant = hitObj.collider.gameObject.GetComponent<Plant>();
                Debug.LogError(currentPlant);
                if (playerInventory.currentItem != null && currentPlant.Type.Equals(playerInventory.currentItem.type))
                    CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                        + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
                playerInventory.Add(currentPlant);
                currentPlant.currentSpace.occupied = false;
                currentPlant.currentSpace.currentPlant = null;
                Destroy(hitObj.collider.gameObject);
            }
            else if (hitObj.collider.tag == "Seed")
            {
                Seed currentSeed = hitObj.collider.gameObject.GetComponent<Seed>();
                if (playerInventory.currentItem != null && currentSeed.Type.Equals(playerInventory.currentItem.type))
                    CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                        + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
                playerInventory.Add(currentSeed);
                currentSeed.currentSpace.occupied = false;
                currentSeed.currentSpace.currentSeed = null;
                Destroy(hitObj.collider.gameObject);
            }
        }

        // I am hijacking this space so we can have all of our user inputs in one place
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerInventory.currentIndex = 0;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && playerInventory.hotbarSize >= 2)
        {
            playerInventory.currentIndex = 1;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && playerInventory.hotbarSize >= 3)
        {
            playerInventory.currentIndex = 2;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 3";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && playerInventory.hotbarSize >= 4)
        {
            playerInventory.currentIndex = 3;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 4";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && playerInventory.hotbarSize >= 5)
        {
            playerInventory.currentIndex = 4;
            
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 5";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && playerInventory.hotbarSize >= 6)
        {
            playerInventory.currentIndex = 5;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 6";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && playerInventory.hotbarSize >= 7)
        {
            playerInventory.currentIndex = 6;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 7";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && playerInventory.hotbarSize >= 8)
        {
            playerInventory.currentIndex = 7;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 8";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && playerInventory.hotbarSize >= 9)
        {
            playerInventory.currentIndex = 8;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 9";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) && playerInventory.hotbarSize >= 10)
        {
            playerInventory.currentIndex = 9;
            if (playerInventory.currentItem != null)
                CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                    + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
            else
                CanvasText.GetComponent<Text>().text = "No item in slot 10";
        }

        // refill the player's inventory
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            GameObject newPlant = Instantiate(TestPlant) as GameObject;
            playerInventory.Add(newPlant.GetComponent<Plant>());
            playerInventory.Add(newPlant.GetComponent<Plant>());
            Destroy(newPlant);

            GameObject newSeed = Instantiate(TestSeed) as GameObject;
            playerInventory.Add(newSeed.GetComponent<Seed>());
            playerInventory.Add(newSeed.GetComponent<Seed>());
            Destroy(newSeed);

            CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
        }

    }

}
