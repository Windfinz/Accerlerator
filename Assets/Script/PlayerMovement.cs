using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] 
    public float walkSpeed; // Tốc độ đi bộ
    [SerializeField]
    public float sprintSpeed; // Tốc độ chạy
    
    private float moveSpeed; // Tốc độ di chuyển

    public float groundDrag; // Ma sát khi đang ở trên mặt đất

    public float jumpForce; // Lực nhảy
    public float jumpCooldown; // Thời gian chờ giữa các lần nhảy
    public float airMultiplier; // Hệ số nhân khi ở trong không gian

    bool readyToJump; // Biến kiểm tra xem có thể nhảy hay không

    Animator animator;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space; // Phím nhảy mặc định là Space

    [Header("Ground Check")]
    public float playerHeight; // Chiều cao của người chơi
    public LayerMask whatIsGround; // LayerMask để xác định mặt đất
    bool grounded; // Biến kiểm tra xem có đang ở trên mặt đất hay không

    public Transform orientation; // Định hướng người chơi

    float horizontalInput; // Input ngang (trái/phải)
    float verticalInput; // Input dọc (trước/sau)

    Vector3 moveDirection; // Hướng di chuyển

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();

        readyToJump = true;
    }

    private void Update()
    {

        // Kiểm tra xem có đang ở trên mặt đất hay không
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput(); // Xử lý input từ người chơi
        SpeedControl(); // Kiểm soát tốc độ
        Sprint();

        // Xử lý ma sát
        if (grounded)
            rb.drag = groundDrag; // Nếu đang ở trên mặt đất, sử dụng ma sát
        else
            rb.drag = 0; // Nếu không, không sử dụng ma sát
    }

    private void FixedUpdate()
    {
        MovePlayer(); // Di chuyển người chơi
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // Nhận input ngang từ bàn phím
        verticalInput = Input.GetAxisRaw("Vertical"); // Nhận input dọc từ bàn phím

        // Kiểm tra xem người chơi có đang di chuyển không
        bool isMoving = horizontalInput != 0 || verticalInput != 0;

        // Cập nhật biến trạng thái của Animator
        animator.SetBool("IsWalking", isMoving);

        // Kiểm tra khi nào được phép nhảy
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false; // Đặt trạng thái nhảy là false

            Jump(); // Thực hiện nhảy

            Invoke(nameof(ResetJump), jumpCooldown); // Gọi hàm ResetJump sau thời gian jumpCooldown
        }
    }

    private void MovePlayer()
    {
        // Lấy hướng của camera trên trục X và Z
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // Tính toán hướng di chuyển dựa trên hướng của camera
        moveDirection = cameraForward * verticalInput + Camera.main.transform.right * horizontalInput;

        // Gán giá trị Y rotation của camera cho Y của player
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Nếu đang ở trên mặt đất
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // Nếu không đang ở trên mặt đất
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }




    private void Sprint()
    {
        // Bỏ điều kiện kiểm tra `grounded`
        if (Input.GetMouseButton(1) && readyToJump && grounded)
        {
            // Tạo một vector hướng dựa trên hướng của camera
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // Tính toán hướng di chuyển
            Vector3 moveDirection = cameraForward;

            // Áp dụng tốc độ chạy
            rb.velocity = moveDirection * sprintSpeed;
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("IsRunning", false);
        }
    }




    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Giới hạn tốc độ nếu cần
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Đặt lại vận tốc theo trục y
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // Thực hiện lực nhảy
    }
    private void ResetJump()
    {
        readyToJump = true; // Đặt trạng thái nhảy là true
    }
}
