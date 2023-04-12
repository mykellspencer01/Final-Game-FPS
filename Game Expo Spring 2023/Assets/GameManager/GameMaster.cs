using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class GameMaster : NetworkComponent
{
    public bool GameStarted = false;
    public bool GameEnded = false;
    public GameObject[] spawns;

   /* public IEnumerator GameEnder()
    {
        GameEnded = true;
        SendUpdate("GAMEENDED", GameEnded.ToString());
        yield return new WaitForSecondsRealtime(5);
    }
   */
    public override void HandleMessage(string flag, string value)
    {
        if (flag == "GAMESTARTED")
        {
            GameStarted = bool.Parse(value);
            if (GameStarted && IsClient)
            {
                foreach (NPM np in GameObject.FindObjectsOfType<NPM>())
                {
                    np.startPanel.transform.gameObject.SetActive(false);
                }
            }
        }
        if (flag == "GAMEENDED")
        {
            GameEnded = bool.Parse(value);
            foreach (NPM np in GameObject.FindObjectsOfType<NPM>())
            {
                np.endPanel.transform.gameObject.SetActive(GameEnded);
                np.gameEnd = true;
            }
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
                bool ready = true;
                int count = 0;
                foreach (NPM np in GameObject.FindObjectsOfType<NPM>())
                {
                    GameStarted = ready;
                    if (!np.IsReady)
                    {
                        ready = false;
                        GameStarted = false;
                        break;
                    }
                    count++;
                }
                if(count < 1)
                {
                    ready = false;
                }
                
                yield return new WaitForSeconds(2);
            }
            SendUpdate("GAMESTARTED", GameStarted.ToString());
            /*if (GameStarted && !GameEnded)
            {
                yield return new WaitForSeconds(5);
                int score1 = Random.Range(100, 350);
                int score2 = Random.Range(100, 350);
                foreach(NPM np in GameObject.FindObjectsOfType<NPM>())
                {
                    np.gameEnd = true;
                    np.SendUpdate("GETSCOREUNO", score1.ToString());
                    np.SendUpdate("GETSCOREDOS", score2.ToString());
                }
                GameEnded = true;
                SendUpdate("GAMEENDED", GameEnded.ToString());
                yield return new WaitForSeconds(5);
                MyCore.UI_Quit();
            }*/
           //spawning the players
            foreach (NPM np in GameObject.FindObjectsOfType<NPM>())
            {
                switch (np.Owner)
                {
                    case 0:
                        MyCore.NetCreateObject(np.CharSelected, np.Owner, GameObject.FindGameObjectWithTag("P1").transform.position, Quaternion.identity);
                        break;
                    case 1:
                        MyCore.NetCreateObject(np.CharSelected, np.Owner, GameObject.FindGameObjectWithTag("P2").transform.position, Quaternion.identity);
                        break;
                    case 2:
                        MyCore.NetCreateObject(np.CharSelected, np.Owner, GameObject.FindGameObjectWithTag("P3").transform.position, Quaternion.identity);
                        break;
                    case 3:
                        MyCore.NetCreateObject(np.CharSelected, np.Owner, GameObject.FindGameObjectWithTag("P4").transform.position, Quaternion.identity);
                        break;
                }
            }

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