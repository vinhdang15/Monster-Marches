using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Tham chiếu đến Text UI
    private float deltaTime = 0.0f;

    private void Awake()
    {
        fpsText = GameObject.Find("FPS_Text").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    
    void Update()
    {
        // Tính toán thời gian giữa các khung hình
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Cập nhật FPS
        int fps = Mathf.CeilToInt(1.0f / deltaTime);
        fpsText.text = "FPS: " + fps;
    }
}