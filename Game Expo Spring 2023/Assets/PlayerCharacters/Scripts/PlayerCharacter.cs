using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerCharacter : NetworkComponent
{
    public Text PlayerName;
    //public Text Score;
    public Material[] MColor;
    public Rigidbody myRig;
    public InputActionAsset myMap;
    public PlayerInput myInput;
    //public Camera playerCam;
    //public Animator myAni;

    public Vector2 lastInput;
    public Vector3 lastDir;
    public Vector3 offset;
    public bool canFire = true;
    public bool lastFire;
    public bool canJump = true;
    public bool lastJump;
    public float speed = 5.0f;
    public float ROF;
    public int ColorSelected = -1;
    public int HP = 100;
    public string PName = "<Default>";

    public Vector2 ParseV2(string v)
    {
        Vector2 temp = new();
        string[] args = v.Trim('(').Trim(')').Split(",");
        temp.x = float.Parse(args[0]);
        temp.y = float.Parse(args[1]);
        return temp;
    }

    public void Move(InputAction.CallbackContext m)
    {
        if (IsLocalPlayer)
        {
            if (m.action.phase == InputActionPhase.Started || m.action.phase == InputActionPhase.Performed)
            {
                SendCommand("MOVE", m.ReadValue<Vector2>().ToString());
            }
            if (m.action.phase == InputActionPhase.Canceled)
            {
                SendCommand("MOVE", Vector2.zero.ToString());
            }
        }
    }

    public void Jump(InputAction.CallbackContext j)
    {
        if (IsLocalPlayer && canJump)
        {
            if (j.action.phase == InputActionPhase.Started || j.action.phase == InputActionPhase.Performed)
            {
                SendCommand("JUMP", true.ToString());
                lastJump = false;
            }
            if (j.action.phase == InputActionPhase.Canceled)
            {
                SendCommand("JUMP", false.ToString());
                lastJump = true;
            }
        }
    }

    public void Fire(InputAction.CallbackContext f)
    {
        if (IsLocalPlayer && canFire)
        {
            if (f.action.phase == InputActionPhase.Started || f.action.phase == InputActionPhase.Performed)
            {
                SendCommand("FIRE", true.ToString());
                //MyCore.NetCreateObject();
                lastFire = false;
            }
            if (f.action.phase == InputActionPhase.Canceled)
            {
                SendCommand("FIRE", false.ToString());
                lastFire = true;
            }
        }
    }

    public IEnumerator FireRate()
    {
        yield return new WaitForSeconds(.05f);
        canFire = true;
        MyCore.NetCreateObject(4, this.Owner, this.transform.position + this.transform.forward * 1.5f, Quaternion.identity);
        SendUpdate("SHOOT", canFire.ToString());
    }

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "MOVE" && IsServer)
        {
            lastInput = ParseV2(value);
        }
        if (flag == "JUMP" && IsServer)
        {
            lastJump = bool.Parse(value);
        }
        if (flag == "FIRE" && IsServer)
        {
            lastFire = bool.Parse(value);
        }
        if (flag == "SHOOT" && IsClient)
        {
            canFire = bool.Parse(value);
        }
    }

    public override void NetworkedStart()
    {
        myRig = GetComponent<Rigidbody>();
        myRig.useGravity = true;
        /*foreach(NPM np in GameObject.FindObjectsOfType<NPM>())
        {
            if(np.Owner == this.Owner)
            {
                PName = (string)np.PName.Clone();
                PlayerName.text = PName;
            }
        }*/
    }

    public override IEnumerator SlowUpdate()
    {
      while(IsConnected)
        {
            foreach (NPM np in GameObject.FindObjectsOfType<NPM>())
            {
                if (np.Owner == this.Owner)
                {
                    yield return new WaitUntil(() => np.IsReady);
                    ColorSelected = np.ColorSelected;
                    switch (ColorSelected)
                    {
                        case 0:
                            this.GetComponent<Renderer>().material = MColor[0];
                            break;
                        case 1:
                            this.GetComponent<Renderer>().material = MColor[1];
                            break;
                        case 2:
                            this.GetComponent<Renderer>().material = MColor[2];
                            break;
                    }
                }
            }
            if(IsServer)
            {
                if (lastFire && canFire)
                {
                    canFire = false;
                    SendUpdate("SHOOT", canFire.ToString());
                    lastFire = false;
                    StartCoroutine(FireRate());
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    void Start()
    {
        //playerCam = Camera.main;
        
    }

    void Update()
    {
        if (IsServer)
        {
            myRig.velocity = new Vector3(lastInput.x, 0, lastInput.y) * speed + new Vector3(0, myRig.velocity.y, 0);
            Debug.Log("Velocity: " + myRig.velocity);
            myRig.angularVelocity = Vector3.zero;
            lastDir = transform.forward;
            transform.forward = new Vector3(lastInput.x, 0, lastInput.y);
            if (lastInput.x == 0 && lastInput.y == 0)
            {
                transform.forward = lastDir;
            }
            else
            {
                transform.forward = new Vector3(lastInput.x, 0, lastInput.y);
            }
            if (lastJump && canJump)
            {
                myRig.velocity += new Vector3(0, 5, 0);
                canJump = false;
            }
            if (!canJump && myRig.velocity.y <= 0)
            {
                RaycastHit info;
                if (Physics.Raycast(this.transform.position, this.transform.up * -1, out info))
                {
                    if (info.distance < 1)
                    {
                        canJump = true;
                    }
                }
            }
            if (HP <= 0)
            {
                MyCore.NetDestroyObject(MyId.NetId);
            }
        }
        if (IsClient)
        {
            //animations
           /* if (IsLocalPlayer)
            {
                playerCam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2,
                    this.transform.position.z - 5);
            }*/
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            HP -= 25;
            Debug.Log("Player HP: " + HP);
        }
    }
}
