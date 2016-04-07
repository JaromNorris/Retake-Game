using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

	public static GameManager control;
	public ArrayList plants;
	public ArrayList spaceNames;
	public GameData data;

	// Use this for initialization
	void Start () {
	}

	void Awake()
	{
		//If not GameManager exists, make this the manager.
		if(control == null)
		{
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		//If one already exists, destroy the current one. (Previous GameManager properly persisted)
		else if(control != this)
		{
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Save()
	{
		BinaryFormatter format = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "savefile1.dat");

		data = new GameData();
		retrieveInfo(ref data);

		format.Serialize(file,data);
		file.Close ();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "savefile1.dat"))
		{
			BinaryFormatter format = new BinaryFormatter();
			FileStream savefile = File.Open (Application.persistentDataPath + "savefile1.dat", FileMode.Open);
			data = (GameData)format.Deserialize(savefile);
			Debug.Log (data.locations.ToString());
			Debug.Log (data.spaceNames.ToString ());
			savefile.Close();
		}
	}

	public void retrieveInfo(ref GameData data)
	{
		GameObject[] spaces;
		spaces = GameObject.FindGameObjectsWithTag("Plantable");

		data.locations = new Transform[spaces.Length];
		data.spaceNames = new String[spaces.Length];
		data.waterPresent = new float[spaces.Length];
		data.pollutionPresent = new float[spaces.Length];
		data.plantTypes = new String[spaces.Length];
		data.daysSincePlanted = new int[spaces.Length];

		for(int i = 0; i < spaces.Length; i++)
		{
			//Save location of plant
			data.locations[i] = spaces[i].transform;
			//The space that it occupies
			data.spaceNames[i] = spaces[i].name;
			//How much water is in that space
			data.waterPresent[i] = spaces[i].GetComponent<Plantable_Space>().waterPresent;
			//How much pollution is in that space
			data.pollutionPresent[i] = spaces[i].GetComponent<Plantable_Space>().pollutionPresent;
			//If plant or seed is not null, save its type and how long since it has been planted
			//If none, save null (don't just skip, everything is correlated by the array index values)
			if(spaces[i].GetComponent<Plantable_Space>().currentPlant != null)
			{
				data.plantTypes[i] = spaces[i].GetComponent<Plantable_Space>().currentPlant.GetComponent<Plant>().species;
				data.daysSincePlanted[i] = spaces[i].GetComponent<Plantable_Space>().currentPlant.GetComponent<Plant>().daysSincePlanted;
			}
			else if(spaces[i].GetComponent<Plantable_Space>().currentSeed != null)
			{
				data.plantTypes[i] = spaces[i].GetComponent<Plantable_Space>().currentPlant.GetComponent<Seed>().species;
				data.daysSincePlanted[i] = spaces[i].GetComponent<Plantable_Space>().currentPlant.GetComponent<Seed>().daysSincePlanted;
			}
			else
			{
				data.plantTypes[i] = null;
				data.daysSincePlanted[i] = 0;
			}
		}

		//Collect information from player's inventory to be saved away
	}
}

[Serializable]
public class GameData
{
	public Transform[] locations;
	public String[] spaceNames;
	public float[] waterPresent;
	public float[] pollutionPresent;
	public String[] plantTypes;
	public int[] daysSincePlanted;

	//Need to add necessary fields to save what is in the player's inventory

}
