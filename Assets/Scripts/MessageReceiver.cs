using UnityEngine;

public class MessageReceiver : MonoBehaviour
{
    public void UDPMessage(string message)
    {
        Debug.Log(message);
    }
}
