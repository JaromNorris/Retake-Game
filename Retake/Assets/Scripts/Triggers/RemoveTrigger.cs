using UnityEngine;
using System.Collections;

public class RemoveTrigger : TriggerTarget
{
    /* The time it will take to destroy the item after it is triggered. */
    public float timeToDestroy;

    /*
     * If the specified trigger behavior is Remove, removes the game object from
     *   the scene and returns true. If not, returns false.
     */
    public override void Trigger(TriggerTarget.TriggerBehavior triggerType)
    {
        if (triggerType != TriggerTarget.TriggerBehavior.Remove)
            return;
        Destroy(gameObject, timeToDestroy);
        PrintMessage();
    }
}
