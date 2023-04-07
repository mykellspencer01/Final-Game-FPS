using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Net_Character_Controller : NetworkComponent
{
    //This will be your network player controller
    public int HP = 10;
    public string playerName;
    // ...etc.

    //Does not have to be synchronized because it will be set to the owner's synchronized value.
    public int Team;
    public int Character;

    public override void HandleMessage(string flag, string value)
    {
        if (IsClient && flag == "NAME")
        {
            playerName = value;
        }
    }

    public override void NetworkedStart()
    {
        
    }

    public override IEnumerator SlowUpdate()
    {
        yield return new WaitForSeconds(.2f);
        foreach (LobbyPlayerScript lp in GameObject.FindObjectsOfType<LobbyPlayerScript>())
        {
            if (lp.Owner == this.Owner)
            {
                yield return new WaitUntil(() => lp.IsReady);
                Team = lp.Team;
                switch (Team)
                {
                    case 0: //RED
                        this.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
                        break;
                    case 1: //BLUE
                        this.GetComponent<Renderer>().material.color = new Color32(0, 0, 255, 255);
                        break;
                    case 2: //GREENW
                        this.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 255);
                        break;
                    case 3: //CYAN
                        this.GetComponent<Renderer>().material.color = new Color32(0, 255, 255, 255);
                        break;
                }
                //playerName = lp.playerName;
                //this.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        yield return new WaitForSeconds(.1f);
        while (IsServer)
        {
            if (IsDirty)
            {
                SendUpdate("NAME", playerName);
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
