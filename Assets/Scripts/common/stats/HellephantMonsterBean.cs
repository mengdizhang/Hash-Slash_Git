using UnityEngine;
using System.Collections;

public class HellephantMonsterBean : CharacterStat
{
		public HellephantMonsterBean ()
		{
				GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).BaseValue = 100;
				GetPrimaryAttribute ((int)CharacterAttributeName.Constitution).UpdateStat ();
				GetVital ((int)VitalName.Health).UpdateStat ();
		}
}
