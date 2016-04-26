using UnityEngine;
using System.Collections;

public class TimeRotation : MonoBehaviour {

	public Light sun;
	public Light moon;
	//Literal seconds we have in an in-game day
	public float secondsInDay = 120f;
	public float currentTime = 0;
	//How much faster night is than our secondsInDay
	public float nightTimeMultiplier = 1f;
	//How much faster day is than our secondsInDay
	public float dayTimeMultiplier= 1f;

	public float timeMultiplier;

	float sunIntensity;
	float moonIntensity;

	// Use this for initialization
	void Start () {
		sunIntensity = sun.intensity;
		moonIntensity = moon.intensity;
	}
	
	// Update is called once per frame
	void Update () {
		sun.transform.localRotation = Quaternion.Euler((currentTime * 360f) - 90, 170, 0);

		float sunMultiplier = 1f;
		float moonMultiplier = 1f;

		//Make sure there's a small gap between time checks for the sun/moon to change intensity
		if (currentTime <= 0.23f || currentTime >= 0.75f) {
			sunMultiplier = 0;
			moonMultiplier = 1;
			timeMultiplier = nightTimeMultiplier;
			RenderSettings.ambientSkyColor = Color.black;
		}
		else if (currentTime <= 0.25f) {
			sunMultiplier = Mathf.Clamp01((currentTime - 0.23f) * (1 / 0.02f));
			moonMultiplier = Mathf.Clamp01(1 - ((currentTime - 0.73f) * (1 / 0.02f)));
			timeMultiplier = dayTimeMultiplier;
		}
		else if (currentTime >= 0.73f) {
			sunMultiplier = Mathf.Clamp01(1 - ((currentTime - 0.73f) * (1 / 0.02f)));
			moonMultiplier = Mathf.Clamp01((currentTime - 0.23f) * (1/0.02f));
			timeMultiplier = dayTimeMultiplier;
		}
		else{
			RenderSettings.ambientSkyColor = Color.gray;
		}

		//Change the intensity of the light based on time of day
		sun.intensity = sunIntensity * sunMultiplier;
		moon.intensity = moonIntensity * moonMultiplier;

		currentTime += (Time.deltaTime /secondsInDay) * timeMultiplier;

		//If day is over, start new day.
		if(currentTime >= 1)
		{
			currentTime = 0;
		}


	}
}
