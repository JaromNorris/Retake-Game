using UnityEngine;
using System.Collections;

public class LevelSwitch : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Application.LoadLevel("Level_1_m");
    }

}
