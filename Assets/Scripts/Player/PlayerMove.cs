using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private InteractiveObjectBase currentInteractable;

    private Animator animator;

    public MaskClass myCurrentMask;
    public AccessoriesType myCurrentAccessory;

    [System.Serializable]
    public struct ModelMap<T>
    {
        public T type;
        public GameObject modelObject;
    }

    public List<ModelMap<MaskClass>> maskModels;
    public List<ModelMap<AccessoriesType>> accessoryModels;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.Translate(-moveDirection * moveSpeed * Time.deltaTime, Space.World);
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
        }

        
        if (animator != null)
        {
            bool isMoving = moveDirection.magnitude > 0.5f;
            animator.SetBool("Run", isMoving);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact(this);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        InteractiveObjectBase obj = other.GetComponent<InteractiveObjectBase>();
        if (obj != null)
        {
            currentInteractable = obj;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractiveObjectBase obj = other.GetComponent<InteractiveObjectBase>();
        if (obj != null && currentInteractable == obj)
        {
            currentInteractable = null;
        }
    }

    //切換面具
    public void ChangeMask(MaskClass newMask)
    {
        myCurrentMask = newMask;

        foreach (var map in maskModels)
        {
            if (map.modelObject != null)
            {
                map.modelObject.SetActive(map.type == newMask);
            }
        }
    }
    //切換飾品
    public void ChangeAccessory(AccessoriesType newAccessory)
    {
        myCurrentAccessory = newAccessory;

        foreach (var map in accessoryModels)
        {
            if (map.modelObject != null)
            {
                map.modelObject.SetActive(map.type == newAccessory);
            }
        }
    }
        
}
