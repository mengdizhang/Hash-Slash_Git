using UnityEngine;
using System.Collections;
using System; //to access to the enum
public enum CharacterType //moster bar or player bar
{
		Monster,
		Player,
}

public class BaseCharacterBean
{
		private string name_;
		private CharacterType characterType;
	
		private UInt32 level_;
		private UInt32 free_exp_;
	
		public CharacterAttribute[] primary_attributes_;
		public VitalBean[] vitals_;
		public SkillBean[] skills_;
	
		public BaseCharacterBean ()
		{
				name_ = string.Empty;
				level_ = 0;
				free_exp_ = 0;
				characterType = CharacterType.Player;
		
				primary_attributes_ = new CharacterAttribute[Enum.GetValues (typeof(CharacterAttributeName)).Length];
				SetupPrimaryAttributes ();
		
				vitals_ = new VitalBean[Enum.GetValues (typeof(VitalName)).Length];
				SetupVitals ();
				SetupVitalModifiers ();
		
				skills_ = new SkillBean[Enum.GetValues (typeof(SkillName)).Length];
				SetupSkills ();
				SetupSkillModifiers ();
		
				UpdateStats (); //update all stats at first based on the base value in each stat
		}
	
		public void UpdateStats () //update the modifying values in vitals and skills
		{
				/*have to update on by one*/
		
				for (int attri_cnt = 0; attri_cnt < primary_attributes_.Length; attri_cnt++) {
						//you must update attri first because other are updated based on attri
						primary_attributes_ [attri_cnt].Update ();
				}
		
				for (int skill_cnt = 0; skill_cnt < skills_.Length; skill_cnt++) {
						//you must update attri dirst because other are updated based on attri
						skills_ [skill_cnt].Update ();
				}
		
				for (int vital_cnt = 0; vital_cnt < vitals_.Length; vital_cnt++) {
						//you must update attri dirst because other are updated based on attri
						vitals_ [vital_cnt].Update ();
				}
		}
	
		public void AddFreeExp (UInt32 exp) //add_exp
		{
				free_exp_ += exp;
				UpdateLevel ();
		}
	
		public void UpdateLevel () //calculate_level
		{
		
		}
	
		public CharacterType CharacterType {
				get { return characterType; }
				set { characterType = value; }
		}
	
		public string Name {
				get { return name_; }
				set { name_ = value; }
		}
	
		public UInt32 Level {
				get { return level_; }
				set { level_ = value; }
		}
	
		public UInt32 FreeExp {
				get { return free_exp_; }
				set { free_exp_ = value; }
		}
	
		private void SetupPrimaryAttributes ()
		{
				for (int cnt = 0; cnt < primary_attributes_.Length; cnt++) {
						primary_attributes_ [cnt] = new CharacterAttribute ();
						primary_attributes_ [cnt].AttriName = (CharacterAttributeName)cnt;
				}
		}
	
		private void SetupSkills ()
		{
				for (int cnt = 0; cnt < skills_.Length; cnt++) {
						skills_ [cnt] = new SkillBean ();
				}
		}
	
		private void SetupVitals ()
		{
				for (int cnt = 0; cnt < vitals_.Length; cnt++) {
						vitals_ [cnt] = new VitalBean ();
				}
		}
	
		public CharacterAttribute GetPrimaryAttribute (int index)
		{
				return primary_attributes_ [index];
		}
	
		public SkillBean GetSkill (int index)
		{
				return skills_ [index];
		}
	
		public VitalBean GetVital (int index)
		{
				return vitals_ [index];
		}
	
		private void SetupVitalModifiers ()
		{
				//Constitution Might -> health
				GetVital ((int)VitalName.Health).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Constitution), 1f));
				GetVital ((int)VitalName.Health).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Might), 1f));
		
				//Constitution -> energy
				GetVital ((int)VitalName.Energy).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Constitution), 1f));
		
				//Willpower -> mana
				GetVital ((int)VitalName.Mana).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Willpower), 1f));
		
				//Constitution -> Physical_Damage
				GetVital ((int)VitalName.Physical_Damage).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Might), 1f));
				//Constitution Might -> Physical_Defence
				GetVital ((int)VitalName.Physical_Defence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Constitution), 1f));
				GetVital ((int)VitalName.Physical_Defence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Might), 1f));
		
				//Willpower -> Magic_Damage
				GetVital ((int)VitalName.Magic_Damage).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Willpower), 1f));
				//Concentration Willpower -> Magic_Defence
				GetVital ((int)VitalName.Magic_Defence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Willpower), 1f));
				GetVital ((int)VitalName.Magic_Defence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Concentration), 1f));
		
				GetVital ((int)VitalName.Energy).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Speed), 2f));
		}
	
		private void SetupSkillModifiers ()
		{
				//Mighyt can increase melle offence
				GetSkill ((int)SkillName.Melee_Offence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Might), 1f));
		
				//Nimbleness can also increase melle offence
				GetSkill ((int)SkillName.Melee_Defence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Nimbleness), 1));
		
				//Concentration and willpower can increase magic defence
				GetSkill ((int)SkillName.Magic_Offence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Concentration), 1));
				GetSkill ((int)SkillName.Magic_Offence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Willpower), 1));
		
				//Only cencentration can increase magic  defence
				GetSkill ((int)SkillName.Magic_Defence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Concentration), 1));
		
				//Concentration and speed can also increase melle offence
				GetSkill ((int)SkillName.Range_Offence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Concentration), 1));
				GetSkill ((int)SkillName.Range_Offence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Speed), 1));
		
				//Nim and speed can also increase melle offence
				GetSkill ((int)SkillName.Range_Defence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Speed), 0.33f));
				GetSkill ((int)SkillName.Range_Defence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Nimbleness), 0.33f));
		
		
				GetSkill ((int)SkillName.Range_Offence).AddModifier (new ModifyingAttribute (GetPrimaryAttribute ((int)CharacterAttributeName.Speed), 2));
		
		}
}
