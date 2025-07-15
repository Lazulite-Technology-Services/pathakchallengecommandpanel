using UnityEngine;

public class MessageReceiver : MonoBehaviour
{
    public UDPClient client;
    public void UDPMessageSend()
    {
        client.SendValue("end");
    }
}
