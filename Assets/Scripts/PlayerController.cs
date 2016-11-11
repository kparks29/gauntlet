using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    Renderer rend;
    Color playerColor;
    public MyLocalPlayer mlp;
    // Use this for initialization
    void Start () {
        if (isLocalPlayer)
        {
            GameObject go = GameObject.FindGameObjectWithTag("LocalPlayer");
            mlp = go.GetComponent<MyLocalPlayer>();
            rend = GetComponent<Renderer>();
            CmdSetup(mlp.myColor);
        }
        //else if (isServer)
        //{
        //    StartCoroutine(ServerSetup());
        //}
	}
	
    void LateUpdate()
    {
        if (isLocalPlayer)
        {
            transform.position = mlp.myHead.position;
            transform.rotation = mlp.myHead.rotation;
        }
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
