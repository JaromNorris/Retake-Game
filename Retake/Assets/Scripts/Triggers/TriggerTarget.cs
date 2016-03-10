using UnityEngine;
using System.Collections;

public abstract class TriggerTarget : MonoBehaviour
{
    /*
     * The types of behaviors that can be activated through a trigger relationship.
     */
    public enum TriggerBehavior { Add, Remove, Move }
    // we can add more of these if needed

    /*
     * Activates a trigger behavior on the target specified by the TriggerObject.
     * Returns true if the trigger type is valid for the target object; false if not.
     */
    public abstract void Trigger(TriggerBehavior triggerType);
}
