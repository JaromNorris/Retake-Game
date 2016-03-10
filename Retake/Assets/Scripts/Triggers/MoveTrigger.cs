using UnityEngine;
using System.Collections;

public class MoveTrigger : TriggerTarget
{
    /* The x-coordinate for placement of the trigger object */
    public float xCoordinate;
    /* The y-coordinate for placement of the trigger object */
    public float yCoordinate;
    /* The z-coordinate for placement of the trigger object */
    public float zCoordinate;    

    /*
     * If the specified trigger behavior is Move, moves the game object to the
     *   specified coordinates, sets the move coordinates to the old coordinates,
     *   and returns true. If not, returns false.
     */
    public override void Trigger(TriggerTarget.TriggerBehavior triggerType)
    {
        if (triggerType != TriggerTarget.TriggerBehavior.Move)
            return;
        Vector3 temp = gameObject.transform.position;
        gameObject.transform.position = new Vector3(xCoordinate, yCoordinate, zCoordinate);
        xCoordinate = temp.x;
        yCoordinate = temp.y;
        zCoordinate = temp.z;
    }
}
