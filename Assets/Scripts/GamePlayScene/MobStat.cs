using UnityEngine;
using System.Collections;

public class MobStat : CharacterStat
{
		// Use this for initialization
		void Start ()
		{
				switch (CharacterType) {
				case "Hellephant":
						Debug.Log ("MobStat -> start() - > switch -> Hellephant");
						GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).BaseValue = 20;
						GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).UpdateStat ();
						UpdateStats ();
						break;
				case "ZomBear":
						Debug.Log ("MobStat -> start() - > switch -> ZomBear");
						GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).BaseValue = 6;
						GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).UpdateStat ();
						UpdateStats ();
						break;
				case "ZomBunny":
						Debug.Log ("MobStat -> start() - > switch -> ZomBunny");
						GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).BaseValue = 7;
						GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).UpdateStat ();
						UpdateStats ();
						break;
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
		}
}

public enum MobTypes //moster bar or player bar
{
		hellephant,
		zoombear,
		zoombunny
}
