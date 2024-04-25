using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public bool rotateAroundSelf = true;
    public bool useRigidbody = true;

    public float moveSpeed = 5f;
    public Vector3 sensitivity = Vector3.one;

    private Rigidbody rb;

    // Used for normal rotation
    [SerializeField] private Vector3 rotation = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");
        float mz = Input.GetAxis("Roll");

        if (rotateAroundSelf)
            RotatePlayerSelf(mx, my, mz);
        else
            RotatePlayer(mx, my, 0);

        if (!useRigidbody)
        {
            float forward = Input.GetAxis("Vertical");
            float right = Input.GetAxis("Horizontal");
            float up = Input.GetAxis("Up");

            TranslatePlayer(right, forward, up);
        }
    }

    void FixedUpdate()
    {
        if (useRigidbody)
        {
            float right = Input.GetAxis("Horizontal");
            float forward = Input.GetAxis("Vertical");
            float up = Input.GetAxis("Up");

            MovePlayer(right, forward, up);
        }
    }

    void RotatePlayer(float x, float y, float z)
    {
        rotation.x += -y;
        rotation.y += x;
        rotation.z += z;

        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotation);
    }

    void RotatePlayerSelf(float x, float y, float z)
    {
        transform.Rotate(new Vector3(-y * sensitivity.x, x * sensitivity.y, -z * sensitivity.z), Space.Self);
    }

    void TranslatePlayer(float right, float forward, float up)
    {
        Vector3 moveDirection = new Vector3(right, up, forward).normalized;
        Vector3 moveAmount = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount, Space.Self);
    }

    void MovePlayer(float right, float forward, float up)
    {
        Vector3 moveDirection = new Vector3(right, up, forward).normalized;
        Vector3 moveAmount = moveDirection * moveSpeed * Time.fixedDeltaTime;

        rb.AddForce(transform.TransformDirection(moveAmount));
    }
}
