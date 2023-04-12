using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NETWORK_ENGINE;


public class NPM : NetworkComponent
{
    public string PName;
    public bool IsReady;
    public int ColorSelected;
    public int CharSelected;

    public bool gameEnd;
    public int score1;
    public int score2;

    public Text[] scores;
    public Text[] names;
    public GameObject startPanel;
    public GameObject endPanel;

    public override void HandleMessage(string flag, string value)
    {
        if(flag == "READY")
        {
            IsReady = bool.Parse(value);
            if(IsServer)
            {
                SendUpdate("READY", value);
            }
        }

        if(flag == "NAME")
        {
            PName = value;
            if(IsServer)
            {
                SendUpdate("NAME", value);
            }
        }

        if(flag =="COLOR")
        {
            ColorSelected = int.Parse(value);
            if(IsServer)
            {
                SendUpdate("COLOR", value);
            }
        }

        if(flag == "CHAR")
        {
            CharSelected = int.Parse(value);
            if(IsServer)
            {
                SendUpdate("CHAR", value);
            }
        }
        /*if (flag == "GAMEOVER")
        {
             gameEnd = bool.Parse(value);
            if (IsServer)
            {
                SendUpdate("GAMEOVER", value);
            }
        }
        if (flag == "GETSCOREUNO")
        {
            score1 = int.Parse(value);
            if (IsServer)
            {
                SendUpdate("GETSCOREUNO", value);
            }
        }
        if (flag == "GETSCOREDOS")
        {
            score2 = int.Parse(value);
            if (IsServer)
            {
                SendUpdate("GETSCOREDOS", value);
            }
        }
        if (flag == "SCORE")
        {
            if (IsClient)
            {
                int index = 0;
                foreach(NPM np in GameObject.FindObjectsOfType<NPM>())
                {
                    names[index].text = np.PName;
                    index++;
                }
                scores[0].text = score1.ToString();
                scores[1].text = score2.ToString();
            }
        }*/
    }
    public void UI_Ready(bool r)
    {
        if(IsLocalPlayer)
        {
            SendCommand("READY", r.ToString());
        }
    }
    public override void NetworkedStart()
    {
       if(!IsLocalPlayer)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void UI_NameInput(string s)
    {
        if(IsLocalPlayer)
        {
            SendCommand("NAME", s);
        }

    }
    public void UI_ColorInput(int c)
    {
        if(IsLocalPlayer)
        {
            SendCommand("COLOR", c.ToString());
        }
    }

    public void UI_CharInput(int c)
    {
        if(IsLocalPlayer)
        {
            SendCommand("CHAR", c.ToString());
        }

    }


    public override IEnumerator SlowUpdate()
    {
        while(IsConnected)
        {
            if(IsServer)
            {
                /*if (gameEnd)
                {
                    SendUpdate("SCORE", gameEnd.ToString());
                }*/
                if(IsDirty)
                {
                    SendUpdate("NAME", PName);
                    SendUpdate("COLOR", ColorSelected.ToString());
                    SendUpdate("CHAR", CharSelected.ToString());
                    /*SendUpdate("GAMEOVER", gameEnd.ToString());
                    SendUpdate("GETSCOREUNO", score1.ToString());
                    SendUpdate("GETSCOREDOS", score2.ToString());*/
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
}
