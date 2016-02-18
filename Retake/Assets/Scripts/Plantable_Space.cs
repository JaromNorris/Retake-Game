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
			currentSeed.GetComponent<Seed>().CurrentSpace = this.gameObject.GetComponent<Plantable_Space>();
			//Spawn GameObject of Seed
			return "Planted " + playerSeed.Species;
		}
		//Else if holding a plant, set that.
		else if(playerPlant)
		{
			currentPlant = playerPlant;
			occupied = true;
			currentPlant.GetComponent<Plant>().CurrentSpace = this.gameObject.GetComponent<Plantable_Space>();
			return "Planted " + playerPlant.Species;
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