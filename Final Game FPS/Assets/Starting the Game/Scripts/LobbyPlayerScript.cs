using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NETWORK_ENGINE;


public class LobbyPlayerScript : NetworkComponent
{
    public int Character;
    public int Team;
    public bool IsReady;

    //UI elements set in Editor
    public Dropdown CharacterSelect;
    public Dropdown TeamSelect;
    public Toggle ReadyToggle;
    public Image MyLobbyBackground;

    public override void HandleMessage(string flag, string value)
    {
        switch (flag)
        {
            case "TEAM":
                Team = int.Parse(value.ToString());
                if (IsServer)
                {
                    SendUpdate("TEAM", value);
                }
                if (IsClient)
                {
                    switch (Team)
                    {
                        case 0://RED
                            MyLobbyBackground.color = new Color32(255, 0, 0, 128);
                            break;
                        case 1://BLUE
                            MyLobbyBackground.color = new Color32(0, 0, 255, 128);
                            break;
                        case 2://GREEN
                            MyLobbyBackground.color = new Color32(0, 255, 0, 128);
                            break;
                    }
                }
                break;
            case "CHAR":
                Character = int.Parse(value);
                if (IsServer)
                {
                    SendUpdate("CHARACTER", value);
                }
                break;
            case "READY":
                IsReady = bool.Parse(value);
                if (IsClient)
                {
                    ReadyToggle.isOn = IsReady;
                }
                if (IsServer)
                {
                    SendUpdate("READY", value);
                   /* if (IsReady)
                    {
                        //Spawns Player's character
                        GameObject temp = MyCore.NetCreateObject(Character, Owner, this.transform.position - new Vector3(0, .5f, 0),
                            Quaternion.identity);

                    }*/
                }
                break;
        }
    }

    public override void NetworkedStart()
    {
        
    }

    public override IEnumerator SlowUpdate()
    {
        if (!IsLocalPlayer)
        {
            ReadyToggle.interactable = false;
            TeamSelect.gameObject.SetActive(false);
            CharacterSelect.gameObject.SetActive(false);
        }
        //Dirty solution just to get the point across, make sure to edit later...
        switch (Owner)
        {
            case 0:
                this.transform.position = new Vector3(-4, 4, 10);
                break;
            case 1:
                this.transform.position = new Vector3(4, 4, 10);
                break;
            case 2:
                this.transform.position = new Vector3(-4, -4, 10);
                break;
            case 3:
                this.transform.position = new Vector3(4, -4, 10);
                break;
        }
        while (IsConnected)
        {
            if (IsLocalPlayer)
            {
                //Input can go here
                //Any local player only notification
            }
            if (IsClient)
            {
                //rarely used, used most in update
            }
            if (IsServer)
            {
                if (IsDirty) //may not need
                {
                    SendUpdate("READY", IsReady.ToString());
                    SendUpdate("CHARACTER", Character.ToString());
                    SendUpdate("TEAM", Team.ToString());
                    IsDirty = false;
                }
            }
            yield return new WaitForSeconds(.1f);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetTeam(int t)
    {
        if (IsLocalPlayer && MyId.IsInit)
        {
            SendCommand("TEAM", t.ToString());
        }
    }

    public void SetCharacter(int c)
    {
        if (IsLocalPlayer && MyId.IsInit)
        {
            SendCommand("CHARACTER", c.ToString());
        }
    }

    public void SetReady(bool r)
    {
        if (IsLocalPlayer && MyId.IsInit)
        {
            SendCommand("READY", r.ToString());
        }
    }
}
