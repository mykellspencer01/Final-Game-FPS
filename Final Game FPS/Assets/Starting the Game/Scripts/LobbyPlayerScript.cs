using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NETWORK_ENGINE;


public class LobbyPlayerScript : NetworkComponent
{
    public string PName;
    public string PScore;
    public bool IsReady;
    public int ColorSelected;
    public int CharSelected;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "READY")
        {
            IsReady = bool.Parse(value);
            if (IsServer)
            {
                SendUpdate("READY", value);
            }
        }

        if (flag == "NAME")
        {
            PName = value;
            if (IsServer)
            {
                SendUpdate("NAME", value);
            }
        }

        if (flag == "COLOR")
        {
            ColorSelected = int.Parse(value);
            if (IsServer)
            {
                SendUpdate("COLOR", value);
            }
        }

        if (flag == "CHAR")
        {
            CharSelected = int.Parse(value);
            if (IsServer)
            {
                SendUpdate("CHAR", value);
            }
        }
    }
    public void UI_Ready(bool r)
    {
        if (IsLocalPlayer)
        {
            SendCommand("READY", r.ToString());
        }
    }
    public override void NetworkedStart()
    {
        if (!IsLocalPlayer)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void UI_NameInput(string s)
    {
        if (IsLocalPlayer)
        {
            SendCommand("NAME", s);
        }

    }
    public void UI_ColorInput(int c)
    {
        if (IsLocalPlayer)
        {
            SendCommand("COLOR", c.ToString());
        }
    }

    public void UI_CharInput(int c)
    {
        if (IsLocalPlayer)
        {
            SendCommand("CHAR", c.ToString());
        }
    }


    public override IEnumerator SlowUpdate()
    {
        while (IsConnected)
        {

            if (IsServer)
            {

                if (IsDirty)
                {
                    SendUpdate("NAME", PName);
                    SendUpdate("COLOR", ColorSelected.ToString());
                    SendUpdate("CHAR", CharSelected.ToString());

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

    // Update is called once per frame
    void Update()
    {

    }
    /*public int Character;
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
                }
                break;
            case "NAME":
                playerName = value;
                if (playerName == "")
                {
                    ReadyToggle.interactable = false;
                }
                else
                {
                    ReadyToggle.interactable = true;
                }
                if (IsServer)
                {
                    SendUpdate("NAME", value);
                }
                
                break;
        }
    }

    public override void NetworkedStart()
    {
        try
        {
            this.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerMenu").transform);
        }
        catch
        {
            Debug.Log("Could not find Player Menu!");
        }
        if (!IsLocalPlayer)
        {
            ReadyToggle.interactable = false;
            TeamSelect.gameObject.SetActive(false);
            CharacterSelect.gameObject.SetActive(false);
            EnterName.gameObject.SetActive(false);
        }
        ReadyToggle.interactable = false;
    }

    public override IEnumerator SlowUpdate()
    {
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
        if (IsLocalPlayer)
        { 
            SendCommand("NAME", s);
        }
    }

    public void SetTeam(int t)
    {
        if (IsLocalPlayer)
        {
            SendCommand("TEAM", t.ToString());
        }
    }

    public void SetCharacter(int c)
    {
        if (IsLocalPlayer)
        {
            SendCommand("CHARACTER", c.ToString());
        }
    }

    public void SetReady(bool r)
    {
        if (IsLocalPlayer)
        {
            SendCommand("READY", r.ToString());
        }
        
    }*/
}
