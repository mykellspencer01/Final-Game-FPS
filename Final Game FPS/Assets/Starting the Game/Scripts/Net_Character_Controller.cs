using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Net_Character_Controller : NetworkComponent
{
    //This will be your network player controller
    public int HP = 10;
    public string Name = "Coolness";
    // ...etc.

    //Does not have to be synchronized because it will be set to the owner's synchronized value.
    public int Team;

    public override void HandleMessage(string flag, string value)
    {
        
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
                    case 0:
                        this.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
                        break;
                    case 1:
                        this.GetComponent<Renderer>().material.color = new Color32(0, 0, 255, 255);
                        break;
                    case 2:
                        this.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 255);
                        break;
                }
            }
        }
        yield return new WaitForSeconds(.1f);
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
