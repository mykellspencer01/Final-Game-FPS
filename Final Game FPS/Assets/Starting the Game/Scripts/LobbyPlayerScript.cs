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
    public string playerName;

    //UI elements set in Editor
    public InputField EnterName;
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
                        case 3://CYAN
                            MyLobbyBackground.color = new Color32(0, 255, 255, 128);
                            break;
                    }
                }
                break;
            case "CHARACTER":
                Character = int.Parse(value);
                if (IsServer)
                {
                    SendUpdate("CHARACTER", value);
                }
                break;
            case "READY":
                IsReady = bool.Parse(value);
                if (IsClient && !IsLocalPlayer)
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
            case "NAME":
                playerName = value;
                if (IsServer)
                {
                    SendUpdate("NAME", value);
                }
                if (IsLocalPlayer && value.Length > 0)
                {
                    ReadyToggle.interactable = true;
                }
                else
                {
                    ReadyToggle.interactable = false;
                }
                break;
        }
    }

    public override void NetworkedStart()
    {
        if (IsClient)
        {
            ReadyToggle.interactable = false;
        }
    }

    public override IEnumerator SlowUpdate()
    {
        if (!IsLocalPlayer)
        {
            ReadyToggle.interactable = false;
            TeamSelect.gameObject.SetActive(false);
            CharacterSelect.gameObject.SetActive(false);
            EnterName.gameObject.SetActive(false);
        }
        try
        {
            this.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerMenu").transform);
        }
        catch
        {
            Debug.Log("Could not find Player Menu!");
        }
        //Dirty solution just to get the point across, make sure to edit later...
        /*switch (Owner)
        {
            case 0:
                this.transform.position = this.transform.position + new Vector3(.5f, 0, 10);
                break;
            case 1:
                this.transform.position = this.transform.position + new Vector3(1f, 0, 10);
                break;
            case 2:
                this.transform.position = this.transform.position + new Vector3(1.5f, 0, 10);
                break;
            case 3:
                this.transform.position = this.transform.position + new Vector3(2f, 0, 10);
                break;
        }*/
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
                    SendUpdate("NAME", playerName);
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

    public void SetName(string s)
    {
        if (MyId != null && IsLocalPlayer)
        { 
            SendCommand("NAME", s);
        }
    }
    public void SetTeam(int t)
    {
        if (MyId != null && IsLocalPlayer)
        {
            SendCommand("TEAM", t.ToString());
        }
    }

    public void SetCharacter(int c)
    {
        if (MyId != null && IsLocalPlayer)
        {
            SendCommand("CHARACTER", c.ToString());
        }
    }

    public void SetReady(bool r)
    {
        if (MyId != null && IsLocalPlayer)
        {
            SendCommand("READY", r.ToString());
        }
        
    }
}
