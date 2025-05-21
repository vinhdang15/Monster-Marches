using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Tham chiếu đến Text UI
    private float deltaTime = 0.0f;
    private bool hasPrepareGame = false;

    public void PrepareGame()
    {
        LoadComponents();
        SetTargetFrameRate();
        hasPrepareGame = true;
    }

    private void LoadComponents()
    {
        fpsText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void SetTargetFrameRate()
    {
        Application.targetFrameRate = 60;
    }
    
    void Update()
    {
        if(!hasPrepareGame) return;
        // Tính toán thời gian giữa các khung hình
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Cập nhật FPS
        int fps = Mathf.CeilToInt(1.0f / deltaTime);
        fpsText.text = "FPS: " + fps;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}