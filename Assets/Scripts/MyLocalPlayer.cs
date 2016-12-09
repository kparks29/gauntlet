using UnityEngine;
using System.Collections;

public class MyLocalPlayer : MonoBehaviour 
{
    public Color myColor = Color.black;
    public Transform myHead;
    public int myint;
	public User user = new User();

    Renderer rend;
    //This will become private
    public bool newCharacter = true;

    public void Start()
    {
        rend = GetComponent<Renderer>();
        DontDestroyOnLoad(gameObject);
    }
    
    public void SetupCharacter(string characterName)
    {
        switch (characterName)
        {
            case "Wizard":
                SetupWizard();
                break;
            case "Warrior":
                SetupWarrior();
                break;
            case "Archer":
                SetupArcher();
                break;
            default:
                SetupWarrior();
                break;
        }
    }

    void SetupWizard()
    {
        if (newCharacter)
        {
            myColor = Color.blue;
        }
        else
        {
            myColor = new Color(myColor.r, myColor.g, myColor.b - 75);
        }
    }

    void SetupWarrior()
    {
        if (newCharacter)
        {
            myColor = Color.red;
        }
        else
        {
            myColor = new Color(myColor.r - 75, myColor.g, myColor.b);
        }
    }

    void SetupArcher()
    {
        if (newCharacter)
        {
            myColor = Color.green;
        }
        else
        {
            myColor = new Color(myColor.r, myColor.g - 75, myColor.b);
        }
    }

    void SetupSkills()
    {

    }

}
