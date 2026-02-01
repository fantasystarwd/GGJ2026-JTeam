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

    public bool disableTag= false;

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
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void Update()
    {
        if (disableTag) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            Transform playerVisual = transform.GetChild(0);
            if (playerVisual != null && moveX != 0)
            {
                Vector3 newScale = playerVisual.localScale;

                newScale.x = (moveX > 0) ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
                playerVisual.localScale = newScale;
            }
        }


        if (animator != null)
        {
            bool isMoving = moveDirection.sqrMagnitude > 0;
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
            currentInteractable.InteractOnEnter(this);
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
        if (myCurrentMask != newMask)
        {
            myCurrentMask = MaskClass.None;
        }
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
        if (myCurrentAccessory != newAccessory)
        {
            myCurrentAccessory = AccessoriesType.None;
        }
        myCurrentAccessory = newAccessory;

        
        foreach (var map in accessoryModels)
        {
            if (map.modelObject != null)
            {
                map.modelObject.SetActive(map.type == newAccessory);
            }
        }
    }

    //禁用操作
    public void DisabledMovement(bool isDisabled)
    {
        disableTag = isDisabled;

        if (disableTag)
        {
            moveDirection = Vector3.zero;
            if (animator != null)
            {
                animator.SetBool("Run", false); 
            }
        }
    }
}
