using UnityEngine;

public enum PlayerJob { None, Chef, Blacksmith }

public class InteractableObj : MonoBehaviour
{
    public PlayerJob requiredJob; 
    public string interactMessage; 

    public void Interact()
    {
        Debug.Log("執行互動: " + interactMessage);
        //InventoryManager.Instance.AddItem(interactMessage);
    }
}
