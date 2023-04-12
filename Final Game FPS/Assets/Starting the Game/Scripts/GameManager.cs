using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class GameManager : NetworkComponent
{
    //Critical variables that must be synchronized
    public bool GameStarted;
    public bool GameEnded;
    public string pn;
    public int score;

    //Score
    //Player Metrics
    //Progress
    //Etc...

    public IEnumerator GameEnd()
    {
        GameEnded = true;
        SendUpdate("GAMEENDED", GameEnded.ToString());
        yield return new WaitForSecondsRealtime(5);
        //bring up a UI
    }

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "GAMESTART" && IsClient)
        {
            GameStarted = bool.Parse(value);
            GameStarted = true;
            GameObject.FindGameObjectWithTag("PlayerMenu").transform.parent.gameObject.GetComponent<Canvas>().enabled = false;
        }
        if(flag == "GAMEENDED" && IsClient)
        {
            GameEnded = bool.Parse(value);
            GameEnded = true;
            //GameObject.FindGameObjectWithTag("EndMenu").enabled = true;
        }
    }

    public override void NetworkedStart()
    {
        
    }

    public override IEnumerator SlowUpdate()
    {
        if (IsServer)
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
        
            //Send Game Ready = true;
            SendUpdate("GAMESTART", GameStarted.ToString());
            //Stop Listening for more players
            //...Function Coming
            //Spawn Players

            foreach (LobbyPlayerScript lp in GameObject.FindObjectsOfType<LobbyPlayerScript>())
            {
                switch(lp.Owner)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
                MyCore.NetCreateObject(lp.Character, lp.Owner, lp.transform.position -
                    new Vector3(0, .5f, 0), Quaternion.identity);
            }
            //StartCoroutine(GameEnd());
            //Display UI
            //StartCoroutine to disconnect users "Disconnect Function" from NetworkCore: transports them back to start menu.
            while (IsServer)
            {

                if (IsDirty)
                {
                    SendUpdate("GAMESTART", GameStarted.ToString());
                    SendUpdate("GAMEENDED", GameEnded.ToString());
                    IsDirty = false;
                }
                yield return new WaitForSeconds(2);
            }
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
