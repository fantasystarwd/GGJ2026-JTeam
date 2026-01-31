using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public MaskClass myCurrentMask;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private InteractiveObjectBase currentInteractable;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact(this);
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
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

    public bool HasObjectType(ObjectType type)
    {
        return InventoryManager.Instance.items.Contains(type.ToString());
    }
}
