using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float zoomSpeed;
    public float moveSpeed;

    public float maxZoom = 1.0f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        MoveCamera(horizontal, vertical);
    }

    void ZoomCamera(float scroll)
    {
        var newSize = Camera.main.orthographicSize + scroll * zoomSpeed;
        if (newSize > maxZoom) {
            Camera.main.orthographicSize = newSize;
        }
    }

    void MoveCamera(float horizontal, float vertical)
    {
        Vector3 moveDirection = new Vector3(horizontal, vertical, 0.0f).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection);
    }
}
