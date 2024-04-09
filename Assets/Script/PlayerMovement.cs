using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // Tốc độ di chuyển cơ bản

    public float groundDrag; // Ma sát khi đang ở trên mặt đất

    public float jumpForce; // Lực nhảy
    public float jumpCooldown; // Thời gian chờ giữa các lần nhảy
    public float airMultiplier; // Hệ số nhân khi ở trong không gian

    bool readyToJump; // Biến kiểm tra xem có thể nhảy hay không

    [HideInInspector] public float walkSpeed; // Tốc độ đi bộ
    [HideInInspector] public float sprintSpeed; // Tốc độ chạy

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

        readyToJump = true;
    }

    private void Update()
    {
        // Kiểm tra xem có đang ở trên mặt đất hay không
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput(); // Xử lý input từ người chơi
        SpeedControl(); // Kiểm soát tốc độ

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
        // Tính toán hướng di chuyển
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Nếu đang ở trên mặt đất
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // Nếu không đang ở trên mặt đất
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
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
