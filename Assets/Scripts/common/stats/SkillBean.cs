using UnityEngine;
using System.Collections;

public class SkillBean : ModifiedStatBean
{
		private bool known_;

		public SkillBean ()
		{
				known_ = false;
				Exp2Nextlevel = GameDatabase.SkillBean_Exp2Nextlevel;
				LevelModifier = GameDatabase.SkillBean_LevelModifier;
		}

		public bool Known {
				get { return known_; }
				set { known_ = value; }
		}
}

public enum SkillName
{
		Melee_Offence,
		Melee_Defence,

		Range_Offence,
		Range_Defence,

		Magic_Offence,
		Magic_Defence
}