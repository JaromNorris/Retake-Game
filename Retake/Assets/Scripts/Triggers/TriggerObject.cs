using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerObject : MonoBehaviour
{
    /* The objects that will respond to the trigger activation. */
    public List<GameObject> targetObjects;
    /* The type of behavior that will affect each target object. */
    public TriggerTarget.TriggerBehavior triggerType;
    // the trigger scripts for each target object
    List<TriggerTarget> targetScripts;

    /*
     * Initializes the TriggerObject by pulling the TriggerTarget behavior from each
     *   target object.
     */
    void Start()
    {
        targetScripts = new List<TriggerTarget>();
        foreach (GameObject o in targetObjects)
            targetScripts.Add(o.GetComponent<TriggerTarget>());
    }

    /*
     * Performs the specified trigger action on each assigned TriggerTarget.
     * Prints a warning message to the log if the trigger behavior is not valid
     *   for the target object.
     */
    public void Activate()
    {
        foreach (TriggerTarget s in targetScripts)
            s.Trigger(triggerType);
    }
}
