using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PlayerJob myJob;         
    public float interactRange = 2f; 

    private Rigidbody rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenBag();
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);
    }

    void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            InteractableObj obj = hit.collider.GetComponent<InteractableObj>();

            if (obj != null)
            {
                if (obj.requiredJob == myJob)
                {
                    obj.Interact();
                }
                else
                {
                    Debug.Log($"職業不符！你需要 {obj.requiredJob} 職業。");
                }
            }
        }
    }

    void OpenBag()
    {
        Debug.Log("打開背包介面");

    }
}