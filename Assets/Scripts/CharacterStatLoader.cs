using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStatLoader : MonoBehaviour 
{

	public Text nameText;
	public Text classText;
	public Text healthText;
	public Text attackText;
	public Text defenseText;
	public Text magicText;

	private CharacterStats characterStats;

	void OnEnable () 
	{
		SetInitialReferences ();
	}

	void OnDisable () 
	{
	
	}

	void Start () 
	{
		if (characterStats != null) 
		{
			nameText.text = characterStats.characterName.ToString();
			classText.text = characterStats.characterClass.ToString();
			healthText.text = characterStats.health.ToString();
			attackText.text = characterStats.attack.ToString();
			defenseText.text = characterStats.defense.ToString();
			magicText.text = characterStats.magic.ToString();
		}
	}

	void Update () {

	}

	void SetInitialReferences () 
	{
		characterStats = GetComponent<CharacterStats> ();
	}
}