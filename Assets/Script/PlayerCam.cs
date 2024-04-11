using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX; // Độ nhạy theo trục X
    public float sensY; // Độ nhạy theo trục Y

    public Transform orientation; // Đối tượng chứa hướng nhìn của người chơi

    float xRotation; // Góc quay theo trục X
    float yRotation; // Góc quay theo trục Y

    private void Start()
    {
        // Khóa con trỏ chuột và ẩn con trỏ chuột khi bắt đầu game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Nhận input từ chuột
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // Tính toán góc quay mới
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f); // Giữ cho góc quay theo trục X không vượt quá -90 độ và 90 độ

        // Quay camera và hướng nhìn của người chơi
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); // Hướng nhìn chỉ quay theo trục Y, không quay theo trục X
    }
}
