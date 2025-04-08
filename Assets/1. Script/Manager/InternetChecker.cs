using UnityEngine;
using UnityEngine.Networking;

public class InternetChecker : MonoBehaviour
{
    public static bool IsConnectedToInternet()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://www.google.com");
        request.SendWebRequest();

        return request.isDone && request.result != UnityWebRequest.Result.ConnectionError;
    }
}