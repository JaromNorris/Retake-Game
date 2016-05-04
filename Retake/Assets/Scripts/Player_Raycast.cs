using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player_Raycast : MonoBehaviour
{
    public float viewDistance;
    public float wateringRate;
    public float pollutionRate;
    public GameObject UICanvas;
    public GameObject waterMask;
    [SerializeField] private AudioClip waterSound;
    [SerializeField] private AudioClip detoxSound;
    [SerializeField] private AudioClip plantSound;
    [SerializeField] private AudioClip pickSound;

    private RaycastHit hitObj;

    public GameObject TestSeed;
    public GameObject TestPlant;

	public int maxWater = 100;
	public int minWater = 0;
	public float currentWater = 100f;

	// Array of all of our prefabs for seeds and plants
	public GameObject[] plantPrefabs;

    public GameObject playerModel;
    private Animator anim;

	ParticleSystem waterParticles;
    private AudioSource audioSource;

    Inventory playerInventory;
    float trueWaterRate;
    float truePollutionRate;

	private UnityEngine.UI.Text rightText;
	private UnityEngine.UI.Text leftText;

	private GameObject lookingAt;

    // Use this for initialization
    void Start()
    {
        playerInventory = GetComponent<Inventory>();
        trueWaterRate = wateringRate * Time.deltaTime;
        truePollutionRate = pollutionRate * Time.deltaTime;
		waterParticles = this.GetComponentInChildren<ParticleSystem>();

		rightText = GameObject.Find ("RightClick").GetComponent<Text>();
		leftText = GameObject.Find ("LeftClick").GetComponent<Text>();

        GameObject inventoryUI = UICanvas.transform.Find("InventoryUI").gameObject;
        inventoryUI.GetComponent<CanvasGroup>().alpha = 0;
        audioSource = GetComponent<AudioSource>();
        anim = playerModel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug view of the ray
        Debug.DrawRay(this.transform.position, this.transform.forward * viewDistance, Color.blue);

		//Looking at object
		if(Physics.Raycast (this.transform.position,this.transform.forward, out hitObj, viewDistance))
		{
			if(hitObj.collider.tag == "Plantable" && hitObj.collider.gameObject.GetComponent<Plantable_Space>())
			{
				if(lookingAt != hitObj.collider.gameObject && lookingAt !=null)
				{
					lookingAt.GetComponent<ParticleSystem>().Stop();
				}
				lookingAt = hitObj.collider.gameObject;

				if(lookingAt.GetComponent<ParticleSystem>().isStopped)
				{
				lookingAt.GetComponent<ParticleSystem>().Play ();
				}
				if(playerInventory.currentItem != null)
				{
				leftText.text = "Plant " + playerInventory.currentItem.prefabName;
				}
			}
			else if(hitObj.collider.tag == "Seed" || hitObj.collider.tag == "Plant")
			{
				rightText.text = "Remove " + hitObj.collider.gameObject.GetComponent<PlantableObject>().name;
			}
			else if(hitObj.collider.tag == "NewSeed")
			{
				rightText.text = "Pick up " + hitObj.collider.gameObject.GetComponent<New_Seed>().name + " seed";
			}

		}
		else
		{
			rightText.text = "";
			leftText.text = "";
			if(lookingAt != null) {
				lookingAt.GetComponent<ParticleSystem>().Stop();
				lookingAt = null;
			}
		}


        // Looking at object and left-click
        // Places an object from inventory if able
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
        {
            anim.SetTrigger("pickup");
            if (hitObj.collider.tag == "Plantable")
            {
                Plantable_Space space;
                if ((space = hitObj.collider.gameObject.GetComponent<Plantable_Space>()) != null && !space.occupied)
                {
                    GameObject plantableObject;
                    if ((plantableObject = playerInventory.Remove(playerInventory.currentIndex)) != null)
                    {
                        plantableObject.transform.position = hitObj.transform.position;
                        plantableObject.transform.rotation = hitObj.transform.rotation;
                        /*
                        if(plantableObject.GetComponent<Animator>().Getc != null)
						    plantableObject.GetComponent<Animator>().SetBool("Active", true);
                         * */

                        audioSource.PlayOneShot(plantSound);
                        if (!space.addPlantableObject(plantableObject))
                            Debug.LogWarning("That object cannot be planted there.");
                    }
                }
            }
        }
        // Looking at object and right-click
        // Interact with an object if possible.
        // Remove an object and add it to inventory if it's a plant or seed.
        else if (Input.GetMouseButtonDown(1) && Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
        {
            anim.SetTrigger("pickup");
            Debug.Log("Selected " + hitObj.collider.name);

            // if the object we're interacting with is a trigger, activate it's effect
            TriggerObject triggerObject;
            if((triggerObject = hitObj.collider.gameObject.GetComponent<TriggerObject>()) != null)
                triggerObject.Activate();

            InventoryEntry entry;
            // We're picking up a new Seed to fill the inventory
            if (hitObj.collider.tag == "NewSeed")
            {
                Debug.Log("hit");
                New_Seed seed;
                //If we want to pick up seeds one by one, simply pull from Resources the prefab and add the Seed script
                if((seed = hitObj.collider.gameObject.GetComponent<New_Seed>()) != null)
                {
                    entry = ScriptableObject.CreateInstance("InventoryEntry") as InventoryEntry;
                    entry.SetInventoryEntry(seed); // this should automatically add the correct number of seeds for clumps of loose seeds
                    playerInventory.Add(entry);
                    Destroy(hitObj.collider.gameObject);
                    audioSource.PlayOneShot(pickSound);
                }
            }
            // we hit a plantable space
            else if (hitObj.collider.tag == "Plantable")
            {
                if ((entry = hitObj.collider.gameObject.GetComponent<Plantable_Space>().removePlantableObject()) != null)
                {
                    playerInventory.Add(entry);
                    audioSource.PlayOneShot(pickSound);
                }
            }
            // we hit a plantable object within a plantable space
            else if (hitObj.collider.tag == "Plant" || hitObj.collider.tag == "Seed")
            {
                PlantableObject currentObject = hitObj.collider.gameObject.GetComponent<PlantableObject>();
                if ((entry = currentObject.currentSpace.removePlantableObject()) != null)
                {
                    playerInventory.Add(entry);
                    audioSource.PlayOneShot(pickSound);
                }   
            }
        }
		
        // Waters plant or seed if in range
        else if (Input.GetKey(KeyCode.Q) && currentWater > 0)
        {
			if(currentWater > minWater)
			{ 
			    currentWater -= trueWaterRate;
                RectTransform rt = waterMask.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y - trueWaterRate);
                if (waterParticles.isStopped)
                    waterParticles.Play();
                if (!waterParticles.enableEmission)
                    waterParticles.enableEmission = true;
                if (audioSource.clip != waterSound || !audioSource.isPlaying)
                {
                    audioSource.clip = waterSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }

                //We hit an object
                if (Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
                {
                    Plantable_Space plantSpace;
                    // we hit a plantable space
                    if (hitObj.collider.tag == "Plantable")
                    {
                        plantSpace = hitObj.collider.gameObject.GetComponent<Plantable_Space>();
                        if (plantSpace.waterPresent < plantSpace.maximumWater)
                            plantSpace.waterPresent += trueWaterRate;
                    }
                }
			}
			else if (currentWater < minWater)
			{
				currentWater = 0;
                waterParticles.enableEmission = false;
                if (audioSource.clip == waterSound)
                    audioSource.Stop();
			}	
        }
		else if(waterParticles.enableEmission)
		{
            waterParticles.enableEmission = false;
            if (audioSource.clip == waterSound)
                audioSource.Stop();
		}
        // Looking at object and pressing 'e'
        // Decontaminates soil if in range
        else if (Input.GetKey(KeyCode.E) && Physics.Raycast(this.transform.position, this.transform.forward, out hitObj, viewDistance))
        {
            Plantable_Space plantSpace;
            // we hit a plantable space
            if (hitObj.collider.tag == "Plantable")
            {
                plantSpace = hitObj.collider.gameObject.GetComponent<Plantable_Space>();
                if (plantSpace.pollutionPresent > 0)
                    plantSpace.pollutionPresent -= truePollutionRate;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            CanvasGroup inventoryCanvasGroup = UICanvas.transform.Find("InventoryUI").gameObject.GetComponent<CanvasGroup>();
            if (inventoryCanvasGroup.alpha == 1)
                inventoryCanvasGroup.alpha = 0;
            else
                inventoryCanvasGroup.alpha = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            playerInventory.currentIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2) && playerInventory.hotbarSize >= 2)
            playerInventory.currentIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3) && playerInventory.hotbarSize >= 3)
            playerInventory.currentIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4) && playerInventory.hotbarSize >= 4)
            playerInventory.currentIndex = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5) && playerInventory.hotbarSize >= 5)
            playerInventory.currentIndex = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha6) && playerInventory.hotbarSize >= 6)
            playerInventory.currentIndex = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha7) && playerInventory.hotbarSize >= 7)
            playerInventory.currentIndex = 6;
        else if (Input.GetKeyDown(KeyCode.Alpha8) && playerInventory.hotbarSize >= 8)
            playerInventory.currentIndex = 7;
        else if (Input.GetKeyDown(KeyCode.Alpha9) && playerInventory.hotbarSize >= 9)
            playerInventory.currentIndex = 8;
        else if (Input.GetKeyDown(KeyCode.Alpha0) && playerInventory.hotbarSize >= 10)
            playerInventory.currentIndex = 9;
    }
}
