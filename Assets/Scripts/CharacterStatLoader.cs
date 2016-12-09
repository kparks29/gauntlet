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
	public Character character;

	void Start () 
	{
		nameText.text = character.character_name;
		classText.text = character.character_class;
		healthText.text = character.max_health.ToString();
		attackText.text = character.strength.ToString();
		defenseText.text = character.defense.ToString();
		magicText.text = character.magic.ToString();
	}
}