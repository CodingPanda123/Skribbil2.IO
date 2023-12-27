using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;


public class ClientScript : MonoBehaviour
{

    Socket socket;

    Thread thread;

    public List<float> squaresDrawnPosition = new List<float>();

    [SerializeField] GameObject square;

    int byteCode;
    bool ThreadEnabled = false;

    int Port;

    string ipAddress;

    byte[] messageBuffer = new byte[2000];


    [SerializeField] TMP_InputField Guess;


    string WordToGuess;
    void Start()
    {

        Port = PlayerPrefs.GetInt("PortNumber");

        ipAddress = PlayerPrefs.GetString("IPAddr");


        IPAddress ipAddr = IPAddress.Parse(ipAddress);

        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, Port);
        socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


        socket.Connect(localEndPoint);

        if (thread == null)
        {
            thread = new Thread(() =>
            {
                while (true)
                {
                    byteCode = socket.Receive(messageBuffer);

                    string message = Encoding.ASCII.GetString(messageBuffer, 0, byteCode);

                    if (message.Contains("Position"))
                    {
                        string[] PositionMessage = message.Replace("Position", "").Replace(":", "").Split(",");

                        foreach (string m in PositionMessage)
                        {
                            squaresDrawnPosition.Add(float.Parse(m));
                        }
                    }

                    else if (message.Contains("WordGuessx61"))
                    {
                        WordToGuess = message.Replace("WordGuessx61",string.Empty);
                    }

                }

            });
        }


        // thread = new Thread(ClientResponse)
        // {
        //     Name = "ClientResponse",
        //     IsBackground = true
        // };



    }

    void ClientResponse()
    {

        if (!thread.IsAlive && !ThreadEnabled)
        {
            thread.Start();
            ThreadEnabled = true;
        }




        if (squaresDrawnPosition.Count == 3)
        {
            Instantiate(square, new Vector3(squaresDrawnPosition[0], squaresDrawnPosition[1], squaresDrawnPosition[2]), Quaternion.identity, transform);
            squaresDrawnPosition.Clear();

        }
        else if (squaresDrawnPosition.Count > 3)
        {
            Instantiate(square, new Vector3(squaresDrawnPosition[0], squaresDrawnPosition[1], squaresDrawnPosition[2]), Quaternion.identity, transform);
            squaresDrawnPosition.RemoveAt(0);
            squaresDrawnPosition.RemoveAt(1);
            squaresDrawnPosition.RemoveAt(2);
        }
        //print(Encoding.UTF8.GetString(messageBuffer, 0, byteCode));

    }

    void Update()
    {
        ClientResponse();

        if (Input.GetKeyDown(KeyCode.Tab) && Guess.text.Replace(" ","").Length > 0)
        {
            if (Guess.text.Replace(" ","").ToLower() == WordToGuess.ToLower())
            {
                SceneManager.LoadScene("You are Correct");
            }else
            {
                Guess.text = string.Empty;
            }
        }
    }



    void OnDestroy()
    {
        thread?.Abort();
        socket?.Close();
    }
}
