using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class ServerScript : MonoBehaviour
{

    Socket socket;

    List<string> wordList = new List<string>
    {
        "Unity",
        "Unity Muse",
        "Unity Sentis",
        "Unity AI",
        "Factory",
        "Cookie",
        "Elephant",
        "Rainbow",
        "Guitar",
        "Island",
        "Dragon",
        "Snowman",
        "Pizza",
        "Sunflower",
        "Banana",
        "Spaceship",
        "Telescope",
        "Octopus",
        "Penguin",
        "Cactus",
        "Tiger",
        "Robot",
        "Dinosaur",
        "Hotdog",
        "Rocket",
        "Watermelon",
        "Soccer",
        "Unicorn",
        "Bicycle",
        "Fireworks",
        "Lighthouse",
        "Toothbrush",
        "Coffee",
        "Jungle",
        "Raincoat",
        "Moonlight",
        "Pirate",
        "Castle",
        "Kangaroo",
        "Piano",
        "Surfing",
        "Flamingo",
        "Mermaid",
        "Crown",
        "Bookshelf",
        "Helicopter",
        "Butterfly",
        "Sausage",
        "Camping",
        "Cupcake",
        "Lemonade",
        "Parrot",
        "Sandcastle",
        "Waterfall",
        "Cowboy",
        "Suitcase",
        "Superhero",
        "Cheeseburger",
        "Volcano",
        "Candle",
        "Squirrel",
        "Ferris Wheel",
        "Submarine",
        "Backpack",
        "Garden",
        "Rainbow",
        "Snail",
        "Whale",
        "Frog",
        "Sunset",
        "Mushroom",
        "Airplane",
        "Racoon",
        "Popcorn",
        "Beehive",
        "Pirate Ship",
        "Windmill",
        "Jellyfish",
        "Ballet",
        "Fireplace",
        "Snowflake",
        "Giraffe",
        "Magnet",
        "Palm Tree",
        "Spider",
        "Spacesuit",
        "Clockwork",
        "Backflip",
        "Gondola",
        "Moustache",
        "Seashell",
        "Waffle",
        "Dolphin",
        "Teapot",
        "Subway",
        "Cucumber",
        "Fountain"
    };

    string Word;
    public List<float> positionBuffer;

    Socket clientSocket;



    int Port;

    string ipAddress;


    byte[] messageBuffer = new byte[2000];
    int byteData;

    bool sentData = false;
    List<Thread> threads = new List<Thread>();
    bool SentWord = false;

    [SerializeField] TMP_Text whatToDraw;

    void Awake()
    {

        // Get Port
        Port = PlayerPrefs.GetInt("PortNumber");
        ipAddress = PlayerPrefs.GetString("IPAddr");


        IPAddress ipAddr = IPAddress.Parse(ipAddress);
        Word = wordList[UnityEngine.Random.Range(0, wordList.Count)];
        whatToDraw.text = $"You have to Draw: {Word}";

        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, Port);
        socket = new Socket(ipAddr.AddressFamily,
        SocketType.Stream, ProtocolType.Tcp);


        socket.Bind(localEndPoint);

        threads.Add(new Thread(() => { socket.Listen(10); }) { Name = "SocketListening", IsBackground = true });
        threads.Add(
            new Thread(() =>
            {
                try
                {
                    clientSocket = socket.Accept();

                }
                catch (SocketException e)
                {
                    if (e.ErrorCode != 10004)
                    {
                        print(e);
                    }
                }
            })
            {
                Name = "SocketAcceptingClient",
                IsBackground = true
            }

        );



    }



    void ServerListening()
    {
        if (clientSocket != null)
        {

            if (clientSocket.Connected && SentWord != true)
            {
                clientSocket.Send(Encoding.ASCII.GetBytes($"WordGuessx61{Word}"));
                SentWord = true;
            }
        }



        foreach (Thread t in threads)
        {
            try
            {

                if (!t.IsAlive)
                {
                    t.Start();
                }
            }
            catch (Exception)
            {
                continue;
            }
        }


        //Debug.Log("Waiting connection ... ");


        if (positionBuffer.Count == 3 && clientSocket != null)
        {
            byte[] position = Encoding.ASCII.GetBytes($"Position: {positionBuffer[0]},{positionBuffer[1]},{positionBuffer[2]}");
            clientSocket.Send(position);
            //print(positionBuffer);

            positionBuffer.Clear();
            sentData = true;
        }


    }

    // Update is called once per frame
    void Update()
    {

        ServerListening();

        if (sentData && clientSocket.Connected == false)
        {
            SceneManager.LoadScene("ClientGuessedit");
        }

    }

    void OnDestroy()
    {
        socket?.Close();
        clientSocket?.Close();

        foreach (Thread t in threads)
        {
            t.Abort();
        }
    }
}
