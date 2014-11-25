using UnityEngine;
using System.Collections;

public class VitalBean : ModifiedStatBean
{
		public VitalBean ()
		{
				//curr_value_ = 0;
				Exp2Nextlevel = GameDatabase.Vitalbean_Exp2Nextlevel;
				LevelModifier = GameDatabase.Vitalbean_LevelModifier;
		}
}

public enum VitalName
{
		Health,
		Mana,
		Energy,
		Physical_Damage,
		Physical_Defence,
		Magic_Defence,
		Magic_Damage
}
