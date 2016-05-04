using UnityEngine;
using System.Collections;

public class LevelSwitch : MonoBehaviour {

    void OnTriggerEnter()
    {
        Application.LoadLevel("Level_1_m");
    }

}
