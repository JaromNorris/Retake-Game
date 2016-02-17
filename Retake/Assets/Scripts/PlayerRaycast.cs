using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour {

	public float viewDistance;
	public GameObject CanvasText;
	private RaycastHit hitObj;

	// Use this for initialization
	void Start () {
		viewDistance = 10;
	
	}
	
	// Update is called once per frame
	void Update () {

		//Position ray from player's view
		Debug.DrawRay (this.transform.position, this.transform.forward * 10, Color.blue);

		//Looking at object and left-click
		if(Input.GetMouseButtonDown(0) && Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
		{
			Debug.Log ("Selected " + hitObj.collider.name);
			CanvasText.GetComponent<Text>().text = hitObj.collider.name;
		}
		//Looking at object and right-click
		else if(Input.GetMouseButtonDown(1) && Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
		{
			Debug.Log ("Removed " + hitObj.collider.name);
		}


	}

}
