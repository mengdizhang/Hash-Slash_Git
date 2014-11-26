using UnityEngine;
using System.Collections;

public class PlayerCharacterStat : BaseCharacterStat
{
		public PlayerCharacterStat ()
		{
				GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).BaseValue = 200;
				GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).Update ();
				GetVital ((int)VitalName.Health).Update ();
		}
}
