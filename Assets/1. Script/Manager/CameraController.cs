using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineConfiner cinemachineConfiner;

    private float camWidth;
    private float camHeight;
    private float maxX;
    private float minX;
    private float maxY;
    private float minY;
    private PolygonCollider2D polygonCollider2D;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 5.4f;
    [SerializeField] private float panSpeed = 0.5f;
    [SerializeField] private float panThreshold = 0.5f;
    private Vector3 touchStart;
    private Vector3 moveStart = new Vector3(0,0,-10);
    private bool hasBoundingShape = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadComponents();
        }
        else
        {
            Destroy(gameObject);
        }
    }    
    private void LoadComponents()
    {
        if(mainCamera == null) mainCamera = GetComponentInChildren<Camera>();
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineConfiner = GetComponentInChildren<CinemachineConfiner>();
    }

    public void ResetBoundingShape(MapDisplayController mapImageController)
    {
        polygonCollider2D = mapImageController.GetPolygonCollider2D();
        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;
        hasBoundingShape = true;

        UpdateCameraSize();
        virtualCamera.transform.position = new Vector3(0, 0, -10);
    }

    private void UpdateCameraSize()
    {
        if(polygonCollider2D == null) return;
        camHeight = virtualCamera.m_Lens.OrthographicSize * 2f;
        camWidth = camHeight * mainCamera.aspect;

        Bounds bounds = polygonCollider2D.bounds;
        minX = bounds.min.x + camWidth/2;
        maxX = bounds.max.x - camWidth/2;

        minY = bounds.min.y + camHeight/2;
        maxY = bounds.max.y - camHeight/2;
    }

    private void Update()
    {
        if(!hasBoundingShape) return;
        if(Input.touchCount == 1)
        {
            HandleTouchPan();
        }
        else if(Input.touchCount == 2)
        {
            HandleTouchZoom();
        }

        // Di chuyển bản đồ bằng chuột
        if(Input.GetMouseButtonDown(0))
        {
            touchStart = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if(Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if(direction.magnitude <= panThreshold)
            {
                moveStart = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                Vector3 moveDirection = moveStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);

                // Giới hạn di chuyển camera trong phạm vi của bounding shape
                Vector3 newPosition = virtualCamera.transform.position + moveDirection * panSpeed;
                newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
                virtualCamera.transform.position = newPosition;
            }
        }

        // Phóng to và thu nhỏ bằng con lăn chuột
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        Zoom(scrollData * zoomSpeed);
    }

    private void Zoom(float increment)
    {
        virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(mainCamera.orthographicSize - increment, minZoom, maxZoom);
        UpdateCameraSize();
    }

    private void HandleTouchInput()
    {
        if(Input.touchCount == 1)
        {
            HandleTouchPan();
        }
        else if(Input.touchCount == 2)
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
    }

    private void HandleTouchPan()
    {
        // Di chuyển bản đồ bằng một ngón tay
        Touch touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Began)
        {
            touchStart = mainCamera.ScreenToWorldPoint(touch.position);
        }
        else if(touch.phase == TouchPhase.Moved)
        {
            Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(touch.position);
            if(direction.magnitude <= panThreshold)
            {
                moveStart = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                Vector3 moveDirection = moveStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);
                virtualCamera.transform.position += moveDirection * panSpeed;
            }
        }
    }

    private void HandleTouchZoom()
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


}