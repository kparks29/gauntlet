using UnityEngine;
using System.Collections;

public class PlatformSwitcher : MonoBehaviour 
{
    public enum Platform { SteamVR, FPS}

    public Platform startingPlatform;
    public GameObject steamVR;
    public GameObject fps;

    MyLocalPlayer mlp;
    BodyParts b;

	// Use this for initialization
	void Start () 
	{
        mlp = GetComponent<MyLocalPlayer>();
        switch (startingPlatform){
            case Platform.SteamVR:
                b = steamVR.GetComponent<BodyParts>();
                steamVR.SetActive(true);
                fps.SetActive(false);
                break;
            case Platform.FPS:
                b = fps.GetComponent<BodyParts>();
                steamVR.SetActive(false);
                fps.SetActive(true);
                break;
            default:
                steamVR.SetActive(false);
                b = fps.GetComponent<BodyParts>();
                fps.SetActive(true);
                break;
        }
        //mlp.myHead = b.head;
	}
}
