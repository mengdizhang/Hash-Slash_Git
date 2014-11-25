using UnityEngine;
using System.Collections;

public class HellephantMonsterBean : BaseCharacterBean
{
	public HellephantMonsterBean()
	{
		GetPrimaryAttribute((int)CharacterAttributeName.Constitution).BaseValue = 100;
		GetPrimaryAttribute((int)CharacterAttributeName.Constitution).Update();
		GetVital((int)VitalName.Health).Update();
	}
}
