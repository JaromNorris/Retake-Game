using UnityEngine;
using System.Collections;

public class Plantable_Space : MonoBehaviour
{
	public bool occupied;
	public int waterPresent;
	public int pollutionPresent;
	public Seed currentSeed;
	public Plant currentPlant;
	public Plantable_Zone parentZone;
	
	void onMouseOver()
	{
		//Show small popup containing info about current space and its zone.
		print(occupied);
		print(waterPresent);
		print(pollutionPresent);
		print(parentZone);
	}

	//For testing purposes we print out plant species
	public string onLeftMouseDown(Seed playerSeed, Plant playerPlant)
	{
		if(occupied)
		{
			return "Space is occupied!";
			// Call remove function?
		}
		//If player is holding seed, set it.
		else if(playerSeed)
		{
			currentSeed = playerSeed;
			occupied = true;
			currentSeed.GetComponent<Seed>().currentSpace = this.gameObject.GetComponent<Plantable_Space>();
			//Spawn GameObject of Seed
			return "Planted " + playerSeed.species;
		}
		//Else if holding a plant, set that.
		else if(playerPlant)
		{
			currentPlant = playerPlant;
			occupied = true;
			currentPlant.GetComponent<Plant>().currentSpace = this.gameObject.GetComponent<Plantable_Space>();
			return "Planted " + playerPlant.species;
		}
		else
		{
			//Player has no plant or seed selected in inventory, nothing happens.
			return null;
		}
		
	}

	void onWaterReceived()
	{
		// Change texture to "wetter" than present and if occupied trigger particles
		
	}

    public bool addPlantableObject(GameObject obj)
    {
        if (occupied)
            return false;
        obj.transform.parent = transform;
        Plant plant;
        Seed seed;
        if ((plant = obj.GetComponent<Plant>()) != null)
        {
            currentPlant = plant;
            currentPlant.currentSpace = this;
        }
        else if ((seed = obj.GetComponent<Seed>()) != null)
        {
            currentSeed = seed;
            currentSeed.currentSpace = this;
        }
        occupied = true;
        return true;
    }

    public InventoryEntry removePlantableObject()
    {
        // get the current object for removal
        PlantableObject deleteObject;
        if (currentPlant != null)
            deleteObject = currentPlant;
        else if (currentSeed != null)
            deleteObject = currentSeed;
        else return null;

        // make sure the plant can be added to the player's inventory after destruction
        InventoryEntry entry = ScriptableObject.CreateInstance("InventoryEntry") as InventoryEntry;
        entry.SetInventoryEntry(deleteObject);

        // remove the object
        Destroy(deleteObject.gameObject);

        // indicate that the space is now empty
        currentSeed = null;
        currentPlant = null;
        occupied = false;
        return entry;
    }
}