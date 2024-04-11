using UnityEngine;

public class LookAtCenter : MonoBehaviour
{
    private Transform playerTransform;
    private Camera mainCamera;

    void Start()
    {
        playerTransform = transform;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Tính toán hướng từ nhân vật đến vị trí giữa màn hình
        Vector3 lookDirection = (mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, mainCamera.nearClipPlane)) - playerTransform.position).normalized;

        // Xoay nhân vật theo hướng nhìn
        playerTransform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }
}
