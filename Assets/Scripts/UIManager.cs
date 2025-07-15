using System;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private UDPClient udpClient;

    [SerializeField]
    private GameObject Player_1_UI_Object, Player_2_UI_Object, loadingObject;

    public GameObject commandPanel;

    [SerializeField]
    private TMP_InputField player_1_Name, player_1_Mobile, player_1_Email, player_2_Name, player_2_Mobile, player_2_Email, ipAddress, portSend, listernPort;

    [SerializeField]
    private Button next_Btn, submit_Btn, comBtn, saveIp;

    private string playerDetails = string.Empty;

    public static Action MessageReceived;
    public static Action StartConnection;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        Init();
    }    

    private void Init()
    {
        instance = this;
        next_Btn.onClick.AddListener(NextPage);
        submit_Btn.onClick.AddListener(Submit);
        comBtn.onClick.AddListener(EnableDisableCommandPanel);
        saveIp.onClick.AddListener(SaveIPAddress);

        MessageReceived += ResetApp;
    }

    private void NextPage()
    {
        Player_1_UI_Object.SetActive(false);
        Player_2_UI_Object.SetActive(true);

        next_Btn.gameObject.SetActive(false);
        submit_Btn.gameObject.SetActive(true);
    }

    private void Submit()
    {
        playerDetails = $"p1/{player_1_Name.text}/{player_1_Mobile.text}/{player_1_Email.text}/-p2/{player_2_Name.text}/{player_2_Mobile.text}/{player_2_Email.text}";
        loadingObject.SetActive(true);

        udpClient.SendValue("game start" + playerDetails);
    }

    public void ResetApp()
    {
        UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
        {
            playerDetails = string.Empty;

            Player_1_UI_Object.SetActive(true);
            Player_2_UI_Object.SetActive(false);

            next_Btn.gameObject.SetActive(true);
            submit_Btn.gameObject.SetActive(false);

            loadingObject.SetActive(false);

            player_1_Name.text = string.Empty;
            player_1_Mobile.text = string.Empty;
            player_1_Email.text = string.Empty;
            player_2_Email.text = string.Empty;
            player_2_Email.text = string.Empty;
            player_2_Email.text = string.Empty;
        });
    }

    private void SaveIPAddress()
    {
        udpClient.portListen = int.Parse(listernPort.text);
        udpClient.portSend = int.Parse(portSend.text);

        PlayerPrefs.SetString("ip", ipAddress.text);
        PlayerPrefs.SetString("listern", listernPort.text);
        PlayerPrefs.SetString("sendport", portSend.text);

        
        EnableDisableCommandPanel();
    }

    private void EnableDisableCommandPanel()
    {
        if(commandPanel.activeSelf)
        {
            commandPanel.SetActive(false);
            if (PlayerPrefs.HasKey("ip"))
            {
                ipAddress.text = PlayerPrefs.GetString("ip");
                listernPort.text = PlayerPrefs.GetString("listern");
                portSend.text = PlayerPrefs.GetString("sendport");
            }

            commandPanel.SetActive(true);


            if (udpClient.client != null)
            {
                Debug.Log("The client is not null");
                udpClient.client.Close();
            }
            else
            {
                Debug.Log("The client is null !!!!!!");
                StartConnection();
            }
        }
        /*else
        {
            if (PlayerPrefs.HasKey("ip"))
            {
                ipAddress.text = PlayerPrefs.GetString("ip");
                listernPort.text = PlayerPrefs.GetString("listern");
                portSend.text = PlayerPrefs.GetString("sendport");
            }

            commandPanel.SetActive(true);


            if (udpClient.client != null)
            {
                Debug.Log("The client is not null");
                udpClient.client.Close();
            }
            else
            {
                Debug.Log("The client is null !!!!!!");
                StartConnection();
            }
                
        }*/
    }
}
