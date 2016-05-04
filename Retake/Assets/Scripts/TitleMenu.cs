using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour {

    public GameObject title;
    public GameObject newGameButton;
    public GameObject newGameText;
    public GameObject loadGameButton;
    public GameObject loadGameText;
    private bool titlefade= false;
    private bool buttonfade = false;
    private float startTime;
    
    // Use this for initialization
	void Start () {
        title.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        newGameButton.GetComponent<Image>().canvasRenderer.SetAlpha(0f);
        loadGameButton.GetComponent<Image>().canvasRenderer.SetAlpha(0f);
        newGameText.GetComponent<Text>().canvasRenderer.SetAlpha(0f);
        loadGameText.GetComponent<Text>().canvasRenderer.SetAlpha(0f);

        StartCoroutine(FadeIn());
	}

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(2);
        startTime = Time.time;
        titlefade = true;
        yield return new WaitForSeconds(4);
        titlefade = false;
        startTime = Time.time;
        buttonfade = true; 
    }
	
	// Update is called once per frame
	void Update () {
	    if (titlefade)
        {
            float t = (Time.time - startTime) / 2f;
            title.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, 1f, t));
        }
        if (buttonfade)
        {
            //float t = (Time.time - startTime) / 2f;
            newGameButton.GetComponent<Image>().CrossFadeAlpha(1.0f, 2f, false);
            loadGameButton.GetComponent<Image>().CrossFadeAlpha(1.0f, 2f, false);
            newGameText.GetComponent<Text>().CrossFadeAlpha(1.0f, 2f, false);
            loadGameText.GetComponent<Text>().CrossFadeAlpha(1.0f, 2f, false);
        }
	}

    public void newGameClick ()
    {
        Application.LoadLevel("Level_1_K");
    }
}
