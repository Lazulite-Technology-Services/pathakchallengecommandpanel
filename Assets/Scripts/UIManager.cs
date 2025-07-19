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

    public static Action<string> MessageReceived;
    public static Action StartConnection;

    public TextMeshProUGUI StatusText;

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

        if (PlayerPrefs.HasKey("ip"))
        {
            ipAddress.text = PlayerPrefs.GetString("ip");
            listernPort.text = PlayerPrefs.GetString("listern");
            portSend.text = PlayerPrefs.GetString("sendport");
        }
    }

    private void NextPage()
    {
        Player_1_UI_Object.SetActive(false);
        Player_2_UI_Object.SetActive(true);

        next_Btn.gameObject.SetActive(false);
        submit_Btn.gameObject.SetActive(true);

        if(player_1_Name.text == "")
        {
            player_1_Name.text = "na";
        }

        if (player_1_Mobile.text == "")
        {
            player_1_Mobile.text = "na";
        }

        if (player_1_Email.text == "")
        {
            player_1_Email.text = "na";
        }
    }

    private void Submit()
    {
        if (player_2_Name.text == "")
        {
            player_2_Name.text = "na";
        }

        if (player_2_Mobile.text == "")
        {
            player_2_Mobile.text = "na";
        }

        if (player_2_Email.text == "")
        {
            player_2_Email.text = "na";
        }


        playerDetails = $"p1/{player_1_Name.text}/{player_1_Mobile.text}/{player_1_Email.text}/-p2/{player_2_Name.text}/{player_2_Mobile.text}/{player_2_Email.text}";
        loadingObject.SetActive(true);

        //playerdts = playerDetails;

        udpClient.SendValue(playerDetails);
    }

    //public string playerdts;

    public void ResetApp(string msg)
    {
        UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
        {
            Debug.Log($"received msg : {msg}");

            if (msg == "end")
            {
                playerDetails = string.Empty;

                player_1_Name.text = string.Empty;
                player_1_Mobile.text = string.Empty;
                player_1_Email.text = string.Empty;
                player_2_Name.text = string.Empty;
                player_2_Email.text = string.Empty;
                player_2_Mobile.text = string.Empty;

                Player_1_UI_Object.SetActive(true);
                Player_2_UI_Object.SetActive(false);

                next_Btn.gameObject.SetActive(true);
                submit_Btn.gameObject.SetActive(false);

                loadingObject.SetActive(false);

                
            }
        });
    }

    private void SaveIPAddress()
    {
        //udpClient.portListen = int.Parse(listernPort.text);
        //udpClient.portSend = int.Parse(portSend.text);

        PlayerPrefs.SetString("ip", ipAddress.text);
        //PlayerPrefs.SetString("listern", listernPort.text);
        //PlayerPrefs.SetString("sendport", portSend.text);

        
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

            //commandPanel.SetActive(true);


            if (udpClient.client != null)
            {
                Debug.Log("The client is not null");
                udpClient.client.Close();
                
            }
            else
            {
                Debug.Log("The client is null !!!!!!");
                //StartConnection();
            }

            StartConnection();
        }
        else
        {
            commandPanel.SetActive(true);
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
