using UnityEngine;
using System.Collections;

public class plant : MonoBehavior
{
	public string species;
	public int size;
}

public class seed : MonoBehavior
{
	public string species;
}

public class plantableZone : MonoBehaviour
{
	public bool occupied;
	public int waterPresent;
	public int pollutionPresent;
	public plantableSpace[] spaces;
	

}

public class plantableSpace : MonoBehaviour : plant
{
	public bool occupied;
	public int waterPresent;
	public int pollutionPresent;
	public seed currentSeed;
	public plant currentPlant;
	public plantableZone parentZone;

	void onMouseOver()
	{
		/* Show small popup containing info about current space and its zone. */
		print(occupied.value());
		print(waterPresent.value());
		print(pollutionPresent.value());
		print(parentZone.value());
	}
	
	void onMouseDown()
	{
		if(occupied)
		{
			print("Space is occupied!");
			// Call remove function?
		}
		else if(seed playerSeed)
		{
			currentSeed = playerSeed;
		}
		else if(plant playerPlant)
		{
			currentPlant = playerPlant;
		}
		/* If not occupied, give option to place seed or plant */
			/* If plant select, spawn plants and occupied = true */
			/* If seed select, spawn seeds and occupied = true */
		/* If occupied, give option to remove plant or seed */
	}
	
	
	
	void onWaterReceived()
	{
		/* Change texture to "wetter" than present and if occupied trigger particles */
	
	}

}