using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySystem : MonoBehaviour
{
    [SerializeField]
    NetworkManagerIso nm;
    [SerializeField]
    Text ip;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    Button StartGame;
    [SerializeField]
    GameObject TribeSelect;
    public PlayerInput pi;


    private void Start()
    {
        nm = GetComponent<NetworkManagerIso>();
        TribeSelect = canvas.transform.Find("TribeSelect").gameObject;
    }

    public void HostGame() 
    {
        nm.StartHost();

        //i = 2 to stop startgame and dc being deactivated;
        for (int i = 2; i < canvas.transform.childCount; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }

        
        //StartButton
        canvas.transform.GetChild(0).gameObject.SetActive(true);
        TribeSelect.SetActive(true);
    }
    
    public void JoinGame() 
    {
        if (ip.text == "")
        {
            nm.networkAddress = "192.168.0.16";
            //something that says enter an ip;
            //return;
        }
        else 
        {
            nm.networkAddress = ip.text;
        }
        nm.StartClient();
        for (int i = 2; i < canvas.transform.childCount; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
        TribeSelect.SetActive(true);
    }

    private void Update()
    {
        if(nm.numPlayers < 2) 
        {
            StartGame.interactable = false;
        }
        else 
        {
            StartGame.interactable = true;
        }
    }

    public void Disconnect() 
    {
        nm.StopServer();
        nm.StopHost();
    }

    public void TribeSelected(int i) 
    {
        pi.guild = i;
        TribeSelect.SetActive(false);
    }
}
