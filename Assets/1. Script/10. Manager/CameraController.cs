using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 5.4f;
    [SerializeField] private float panSpeed = 0.5f;

    private Vector3 touchStart;

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            // Di chuyển bản đồ bằng một ngón tay
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = mainCamera.ScreenToWorldPoint(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(touch.position);
                mainCamera.transform.position += direction * panSpeed;
            }
        }
        else if (Input.touchCount == 2)
        {
            // Phóng to và thu nhỏ bằng hai ngón tay
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * zoomSpeed);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // Di chuyển bản đồ bằng chuột
            touchStart = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mainCamera.transform.position += direction * panSpeed;
        }

        // Phóng to và thu nhỏ bằng con lăn chuột
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        Zoom(scrollData * zoomSpeed * 100f);
    }

    private void Zoom(float increment)
    {
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - increment, minZoom, maxZoom);
    }
}