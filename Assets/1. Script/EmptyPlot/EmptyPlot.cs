using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class EmptyPlot : MonoBehaviour
{
    public float X { get; private set; }
    public float Y { get; private set; }

    public EmptyPlot(float x, float y)
    {
        X = x;
        Y = Y;
    }

    public Vector2 GetPos()
    {
        return transform.position;
    }

    internal void HideEmptyPlot()
    {
        gameObject.SetActive(false);
    }
    internal void ShowEmptyPlot()
    {
        gameObject.SetActive(true);
    }
}
