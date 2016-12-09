using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour {

	public GameObject characters;
	public GameObject characterPrefab;

	public Material archerMaterial;
	public Material wizardMaterial;
	public Material warriorMaterial;

	public Character baseArcher;
	public Character baseWizard;
	public Character baseWarrior;

	private MyLocalPlayer localPlayer;

	// parameters for when on the left side
	private float maxSideX = 1.82f;
	private float minSideX = -1.95f;
	private float maxSideZ = 6.42f;
	private float minSideZ = 2.66f;

	// parameters for when on the front
	private float maxFrontX = 6.18f;
	private float minFrontX = 3.05f;
	private float maxFrontZ = 2.37f;
	private float minFrontZ = -3.48f;

	void OnEnable ()
	{
		SetInitialReferences ();
	}
		
	public void SpawnCharacters () {
		if (localPlayer.newCharacter) 
		{
			SpawnNewCharacters ();
		}
		else
		{
			SpawnExistingCharacters ();
		}
	}

	void SpawnNewCharacters ()
	{
		if (localPlayer.newCharacter)
		{
			// spawn archer
			Vector3 archerPosition = new Vector3 (-3.75f, 0.02f, -0.18f);
			Vector3 archerRotation = new Vector3 (0, -90f, 0);
			CharacterOptionSettings (archerPosition, archerRotation, archerMaterial, baseArcher);

			// spawn wizard
			Vector3 wizardPosition = new Vector3 (-1.14f, 0.02f, 3.001f);
			Vector3 wizardRotation = new Vector3 (0, 0, 0);
			CharacterOptionSettings (wizardPosition, wizardRotation, wizardMaterial, baseWizard);

			// spawn warrior
			Vector3 warriorPosition = new Vector3 (2.25f, 0.02f, 0.29f);
			Vector3 warriorRotation = new Vector3 (0, 90f, 0);
			CharacterOptionSettings (warriorPosition, warriorRotation, warriorMaterial, baseWarrior);

		}
	}

	void SpawnExistingCharacters ()
	{
		foreach (Character character in localPlayer.user.characters)
		{
			float randomValue = Random.value;
			string position;
			Vector3 characterPosition = new Vector3(0, 0, 0);
			Vector3 characterRotation = new Vector3(0, 0, 0);
			Material material = archerMaterial;

			if (randomValue <= 0.25f)
			{
				// left position 
				characterPosition = new Vector3 (Random.Range(minSideX, maxSideX), 0.02f, Random.Range(minSideZ, maxSideZ));
				characterRotation = new Vector3 (0, 0, 0);
			}
			else if (randomValue > 0.25f && randomValue <= 0.5f)
			{
				// right position
				characterPosition = new Vector3 (Random.Range(minSideX * -1, maxSideX * -1), 0.02f, Random.Range(minSideZ * -1, maxSideZ * -1));
				characterRotation = new Vector3 (0, 180f, 0);
			}
			else if (randomValue > 0.5f && randomValue <= 0.75f)
			{
				// down position 
				characterPosition = new Vector3 (Random.Range(minFrontX, maxFrontX), 0.02f, Random.Range(minFrontZ, maxFrontZ));
				characterRotation = new Vector3 (0, 90, 0);
			}
			else if (randomValue > 0.75f)
			{
				// down position
				characterPosition = new Vector3 (Random.Range(minFrontX * -1, maxFrontX * -1), 0.02f, Random.Range(minFrontZ * -1, maxFrontZ * -1));
				characterRotation = new Vector3 (0, -90, 0);
			}

			if (character.character_class == "Archer")
			{
				material = archerMaterial;
			}
			else if (character.character_class == "Wizard")
			{
				material = wizardMaterial;
			}
			else if (character.character_class == "Warrior")
			{
				material = warriorMaterial;
			}

			CharacterOptionSettings (characterPosition, characterRotation, material, character);
		}
	}
		
	void SetInitialReferences ()
	{
		localPlayer = GameObject.FindObjectOfType<MyLocalPlayer> ();
	}

	void CharacterOptionSettings (Vector3 position, Vector3 rotation, Material material, Character character)
	{
		GameObject newCharacter = Instantiate (characterPrefab, position, Quaternion.Euler(rotation));
		newCharacter.transform.parent = characters.transform;
		var newCharacterRenderer = newCharacter.GetComponent<MeshRenderer> ();
		var newCharacterStats = newCharacter.GetComponent<CharacterStatLoader>();
		newCharacterRenderer.material = material;
		newCharacterStats.character = character;
	}
}
