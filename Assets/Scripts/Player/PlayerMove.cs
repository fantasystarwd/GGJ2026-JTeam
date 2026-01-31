using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public MaskClass myCurrentMask;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private InteractiveObjectBase currentInteractable;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
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
            animator.SetFloat("Run", moveDirection.magnitude);
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
}
