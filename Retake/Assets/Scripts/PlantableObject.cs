using UnityEngine;
using System.Collections;

public abstract class PlantableObject : MonoBehaviour
{
    public string species;
    public int currentSize;
    public int maxSize;
    public int lightRequired;
    public int waterRequired;
	public int pollutionRequired;
    public Plantable_Space currentSpace;

	public GameObject nextGrowth;
	private ParticleSystem growthParticles;
	public GameObject Sun;

	public float timePlanted;
	public float secondsInDay;

	public int daysSincePlanted;
	public int daysToMature;

	void Start()
	{
		//Debug.Log ("Started Life");
		secondsInDay = Sun.GetComponent<TimeRotation>().secondsInDay;
		daysSincePlanted = 0;
		growthParticles = this.GetComponent<ParticleSystem>();
		timePlanted = Sun.GetComponent<TimeRotation>().currentTime;
		Debug.Log ("Got Stats");
		//Wait until the equivalent of one day has passed
		//Repeat every '24 hours'
		InvokeRepeating("CountDay", secondsInDay, secondsInDay);
	}

	/*
	 * Although normally we'd set things in Start(), technically Awake() will be called
	 * before Start() is on an instantiated object (such as our Plants)
	 */
	void Awake()
	{
		Sun = GameObject.Find("Sun");
	}

	/*
	 *  Returns if the plant has reached requirements to Grow to its next stage, if such exists
	 * 
	 */
	public bool growthRequirements()
	{
		//Has it been long enough for it to grow?
		if(daysSincePlanted >= daysToMature)
		{
			//Do we have the required Water and (Lack thereof) Pollution?
			if(currentSpace.GetComponent<Plantable_Space>().waterPresent >= waterRequired && currentSpace.GetComponent<Plantable_Space>().pollutionPresent <= pollutionRequired)
			{
				//Plant is capable of growing! Unless we have additional requirements
				return true;
			}
			else
			return false;
		}
		else
			return false;
	}

	public void Grow()
	{
		if(nextGrowth)
		{
		//Start the growth particle process
			Debug.Log("Growing");
			growthParticles.Play();
			Debug.Log("Played");
		//Wait until the particles are done
		//*******IMPORTANT******** THE PARTICLE SYSTEM SHOULD NOT LOOP
		StartCoroutine(finishGrowth(growthParticles.duration));
		}
	}

	IEnumerator finishGrowth(float time)
	{
		yield return new WaitForSeconds(time);

		//After particles are done, place next Plant in the same position
		GameObject grew = Instantiate(nextGrowth, this.transform.position, this.transform.rotation) as GameObject;
		currentSpace.GetComponent<Plantable_Space>().currentPlant = grew.GetComponent<Plant>();
		grew.GetComponent<PlantableObject>().currentSpace = currentSpace;
		//Destroy this plant.
		Destroy (this.gameObject);
	}

	void CountDay()
	{
		daysSincePlanted++;

		if(growthRequirements ())
		{
			Grow();
		}
	}

    public abstract string Type { get; }
}
