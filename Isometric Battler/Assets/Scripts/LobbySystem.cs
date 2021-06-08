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
    [SerializeField]
    GameObject MapSelect;
    GameManager gm;

    private void Start()
    {
        nm = GetComponent<NetworkManagerIso>();
        //TribeSelect = canvas.transform.Find("TribeSelect").gameObject;
    }

    public void HostGame() 
    {
        nm.StartHost();

        //i = 2 to stop startgame and dc being deactivated;
        canvas.transform.Find("Start").gameObject.SetActive(false);
        canvas.transform.Find("Host").gameObject.SetActive(false);
        canvas.transform.Find("Join").gameObject.SetActive(false);
        //StartButton
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
    }

    IEnumerator ReadyEnable() 
    {
        gm = FindObjectOfType<GameManager>();
        while (gm.teams[0].things.Count < 1 || gm.teams[1].things.Count < 1) 
        {
            StartGame.interactable = false;
            Debug.Log("No interactable");

            yield return null;

        }
        Debug.Log("Interactable");
        Debug.Log(gm.teams[0].things.Count + "    " + gm.teams[1].things.Count);
        StartGame.interactable = true;
    }

    public void Disconnect()
    {
        nm.StopServer();
        nm.StopHost();
    }

    public void TribeSelected(int i) 
    {
        TribeSelect.SetActive(false);
        pi.TribeSelected(i);
        pi.loaded = true;
        pi.StartCoroutine("GhostBuild");
        StartCoroutine(ReadyEnable());
    }

    
}
