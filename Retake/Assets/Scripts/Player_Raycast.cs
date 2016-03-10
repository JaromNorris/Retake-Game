using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player_Raycast : MonoBehaviour
{

    public float viewDistance;
    public GameObject CanvasText;
    private RaycastHit hitObj;

    public GameObject TestSeed;
    public GameObject TestPlant;

	//Array of all of our prefabs for seeds and plants
	public GameObject[] plantPrefabs;

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
        // Debug view of the ray
        Debug.DrawRay(this.transform.position, this.transform.forward * viewDistance, Color.blue);

        // Looking at object and left-click
        // Places an object from inventory if able
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
                        planted = Instantiate(Resources.Load ("TestPlant"), hitObj.transform.position, hitObj.transform.rotation) as GameObject;
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
			//We're picking up a new Seed to fill the inventory
			else if(hitObj.collider.tag == "NewSeed")
			{
				/*
				 * If we want to pick up seeds one by one, simply pull from Resources the prefab and add the Seed script
				GameObject newSeed = Instantiate(Resources.Load(hitObj.collider.gameObject.GetComponent<New_Seed>().name)) as GameObject;
				playerInventory.Add(newSeed.GetComponent<Seed>());
				*
				*If we want to be able to grab them in bunches (One new seed object holds multiple seeds)
1				GameObject newSeed = Instantiate(Resources.Load(hitObj.collider.gameObject.GetComponent<New_Seed>().name)) as GAmeObject;
				for(int i = 0; i < newSeed.GetComponent<New_Seed>().number_of; i++)
				{
				playerInventory.Add(newSeed.GetComponent<Seed>());
				}
				*/
			}
        }
        // Looking at object and right-click
        // Interact with an object if possible.
        // Remove an object and add it to inventory if it's a plant or seed.
        else if (Input.GetMouseButtonDown(1) && Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
        {
            Debug.Log("Selected " + hitObj.collider.name);

            // if the object we're interacting with is a trigger, activate it's effect
            TriggerObject triggerObject;
            if((triggerObject = hitObj.collider.gameObject.GetComponent<TriggerObject>()) != null)
                triggerObject.Activate();

            // we hit a plantable space
            if (hitObj.collider.tag == "Plantable")
            {
                // there is an object in the plantable space
                Plantable_Space plantSpace;
                if ((plantSpace = hitObj.collider.gameObject.GetComponent<Plantable_Space>()).occupied)
                {
                    plantSpace.occupied = false;
                    // the object is a plant
                    if (plantSpace.currentPlant != null)
                    {
                        // add the plant to the player's inventory
                        Plant currentPlant = plantSpace.currentPlant;
                        playerInventory.Add(currentPlant);
                        if(playerInventory.currentItem != null && currentPlant.Type.Equals(playerInventory.currentItem.type))
                            CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                                + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
                        // remove the plant object from the scene
                        Destroy(currentPlant.gameObject);
                    }
                    // the object is a seed
                    else if (plantSpace.currentSeed != null)
                    {
                        // add the seed to the player's inventory
                        Seed currentSeed = plantSpace.currentSeed;
                        playerInventory.Add(currentSeed);
                        if (playerInventory.currentItem != null && currentSeed.Type.Equals(playerInventory.currentItem.type))
                            CanvasText.GetComponent<Text>().text = "Current item: " + playerInventory.currentItem.species + " "
                                + playerInventory.currentItem.type + " (" + playerInventory.currentItem.count + ")";
                        // remove the seed object from the scene
                        Destroy(currentSeed.gameObject);
                    }
                    // indicate that the space is now empty
                    plantSpace.currentSeed = null;
                    plantSpace.currentPlant = null;
                }
            }
            // we hit a plant
            else if (hitObj.collider.tag == "Plant")
            {
                Plant currentPlant = hitObj.collider.gameObject.GetComponent<Plant>();
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
