using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerClientTransition : MonoBehaviour
{
    [SerializeField] private int Port;
    [SerializeField] private string IPAddress;
    [SerializeField] private TMP_InputField PortInput;
    [SerializeField] private TMP_InputField IPAddrInput;

    void SetPortandIPAddr(int defaultPort=5050)
    {
        Port = (PortInput.text.Length > 0) ? int.Parse(PortInput.text) : defaultPort;
        IPAddress = (IPAddrInput.text.Replace(" ","").Length == 13) ? IPAddrInput.text.Replace(" ","") : throw new System.Exception("Invalid IPAddr");
        PlayerPrefs.SetInt("PortNumber", Port);
        PlayerPrefs.SetString("IPAddr", IPAddress);
    }
    public void HostServer()
    {
        SetPortandIPAddr();
        SceneManager.LoadScene("Server");

    }

    public void ConnectClient()
    {
        SetPortandIPAddr();
        SceneManager.LoadScene("Client");

    }
}
