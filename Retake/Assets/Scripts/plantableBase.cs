/*
 * 
 * For the time being all classes here have been split into separate files
 * to ease creating multiple objects.
using UnityEngine;
using System.Collections;

public class plant : MonoBehaviour
{
	public string species;
	public int size;
}

public class seed : MonoBehaviour
{
	public string species;
	public int max_growth_size;
}

public class plantableZone : MonoBehaviour
{
	public bool occupied;
	public int waterPresent;
	public int pollutionPresent;
	public plantableSpace[] spaces;

}

public class plantableSpace : MonoBehaviour
{
	public bool occupied;
	public int waterPresent;
	public int pollutionPresent;
	public seed currentSeed;
	public plant currentPlant;
	public plantableZone parentZone;

	void onMouseOver()
	{
		//Show small popup containing info about current space and its zone.
		print(occupied);
		print(waterPresent);
		print(pollutionPresent);
		print(parentZone);
	}
	
	string onLeftMouseDown(seed playerSeed, plant playerPlant)
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
			//Spawn GameObject of Seed
			return "Planted " + playerSeed.species + "!";
		}
		//Else if holding a plant, set that.
		else if(playerPlant)
		{
			currentPlant = playerPlant;
			return "Planted " + playerPlant.species + "!";
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

}
*/