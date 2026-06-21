using UnityEngine;

//Instance of an interactable object that can be interacted with by the player
public class InteractableObjectInstance : Interactable
{
    //Change the contents of this method to define what happens when the player interacts with this object
    public override void Interact()
    {
        Debug.Log("Object Is Begin Interacted With");
    }
}