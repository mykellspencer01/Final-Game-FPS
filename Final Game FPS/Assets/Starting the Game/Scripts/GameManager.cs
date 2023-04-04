using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class GameManager : NetworkComponent
{
    //Critical variables that must be synchronized
    public bool GameStarted;

    //Score
    //Player Metrics
    //Progress
    //Etc...

    public override void HandleMessage(string flag, string value)
    {
        //This will only happen client side
        if (flag == "GAMESTART")
        {
            GameStarted = true;
            foreach (LobbyPlayerScript lp in GameObject.FindObjectsOfType<LobbyPlayerScript>())
            {
                lp.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public override void NetworkedStart()
    {
        
    }

    public override IEnumerator SlowUpdate()
    {
        while (!GameStarted && IsServer)
        {
            bool readyGo = true;
            int count = 0;
            foreach (LobbyPlayerScript lp in GameObject.FindObjectsOfType<LobbyPlayerScript>())
            {
                if (!lp.IsReady)
                {
                    readyGo = false;
                    break;
                }
                count++;
            }
            if (count < 1)
            {
                readyGo = false;
            }
            GameStarted = readyGo;
            yield return new WaitForSeconds(2);
        }
        if (IsServer)
        {
            //Send Game Ready = true;
            SendUpdate("GAMESTART", GameStarted.ToString());
            //Stop Listening for more players
            //...Function Coming
            foreach (LobbyPlayerScript lp in GameObject.FindObjectsOfType<LobbyPlayerScript>())
            {
                MyCore.NetCreateObject(lp.Character, lp.Owner, lp.transform.position -
                    new Vector3(0, .5f, 0), Quaternion.identity);
            }

            //Spawn Players
        }
        while (IsServer)
        {
            if (IsDirty)
            {
                SendUpdate("GAMESTART", GameStarted.ToString());
                IsDirty = false;
            }
            yield return new WaitForSeconds(5);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
