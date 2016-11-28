using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    Renderer rend;
    [SyncVar]
    public Color playerColor;
    public MyLocalPlayer mlp;
    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        if (isLocalPlayer)
        {
            GameObject go = GameObject.FindGameObjectWithTag("LocalPlayer");
            mlp = go.GetComponent<MyLocalPlayer>();
            CmdSetup(mlp.myColor);
        }
        else
        {
            Setup(playerColor);
        }
    }
	
    void LateUpdate()
    {
        if (isLocalPlayer)
        {
            transform.position = mlp.myHead.position;
            transform.rotation = mlp.myHead.rotation;
        }
    }

    void Setup(Color color)
    {
        rend.material.color = color;
    }

    IEnumerator ServerSetup()
    {
        yield return new WaitForSeconds(1.5f);
        CmdSetup(playerColor);
    }

    [Command]
    void CmdSetup(Color c)
    {
        RpcSetup(c);
    }
    
    [ClientRpc]
    void RpcSetup(Color c)
    {
        rend.material.color = c;
        playerColor = c;
    }

}
