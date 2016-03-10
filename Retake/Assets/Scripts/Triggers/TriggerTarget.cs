using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class TriggerTarget : MonoBehaviour
{
    /*
     * The types of behaviors that can be activated through a trigger relationship.
     */
    public enum TriggerBehavior { Add, Remove, Move }
    // we can add more of these if needed

    public string triggerMessage;
    public GameObject CanvasText;

    /*
     * Activates a trigger behavior on the target specified by the TriggerObject.
     * Returns true if the trigger type is valid for the target object; false if not.
     */
    public abstract void Trigger(TriggerBehavior triggerType);

    protected void PrintMessage()
    {
        if(!triggerMessage.Equals("") && CanvasText != null)
            CanvasText.GetComponent<Text>().text = triggerMessage;
    }
}
