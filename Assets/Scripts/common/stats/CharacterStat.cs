﻿using UnityEngine;
using System.Collections;
using System; //to access to the enum

public class CharacterStat : MonoBehaviour
{
		public string characterType;
		public string name_;
		public int level_;
		public int free_exp_;
		public CharacterAttribute[] primary_attributes_;
		public VitalBean[] vitals_;
		public SkillBean[] skills_;

		void Awake ()
		{
				//Debug.Log ("CharacterStat -> Awake()");
				name_ = string.Empty;
				level_ = 0;
				free_exp_ = 0;
				//characterType = PlayerTypes.Player;
		
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
		void Start ()
		{
				//Debug.Log ("CharacterStat -> Start()");
		}

	
		public void UpdateStats () //update the modifying values in vitals and skills
		{
				/*have to update on by one*/
		
				for (int attri_cnt = 0; attri_cnt < primary_attributes_.Length; attri_cnt++) {
						//you must update attri first because other are updated based on attri
						primary_attributes_ [attri_cnt].UpdateStat ();
				}
		
				for (int skill_cnt = 0; skill_cnt < skills_.Length; skill_cnt++) {
						//you must update attri dirst because other are updated based on attri
						skills_ [skill_cnt].UpdateStat ();
				}
		
				for (int vital_cnt = 0; vital_cnt < vitals_.Length; vital_cnt++) {
						//you must update attri dirst because other are updated based on attri
						vitals_ [vital_cnt].UpdateStat ();
				}
		}
	
		public void AddFreeExp (int exp) //add_exp
		{
				free_exp_ += exp;
				UpdateLevel ();
		}
	
		public void UpdateLevel () //calculate_level
		{
		
		}
	
		public string CharacterType {
				get { return characterType; }
				set { characterType = value; }
		}
	
		public string Name {
				get { return name_; }
				set { name_ = value; }
		}
	
		public int Level {
				get { return level_; }
				set { level_ = value; }
		}
	
		public int FreeExp {
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

		public void Copy (CharacterStat stat)
		{
				name_ = stat.Name;
				level_ = stat.Level;
				free_exp_ = stat.FreeExp;
				for (int attri_cnt = 0; attri_cnt < primary_attributes_.Length; attri_cnt++) {
						//you must update attri first because other are updated based on attri
						primary_attributes_ [attri_cnt].BaseValue = stat.GetPrimaryAttribute (attri_cnt).BaseValue;
						primary_attributes_ [attri_cnt].Buff_Value = stat.GetPrimaryAttribute (attri_cnt).Buff_Value;
						primary_attributes_ [attri_cnt].TotalValue = stat.GetPrimaryAttribute (attri_cnt).TotalValue;
				}
				UpdateStats ();
		}

		public void ToStr ()
		{
				Debug.Log ("Name: " + name_);
				for (int cnt = 0; cnt < primary_attributes_.Length; cnt++) {
						Debug.Log ("AttriName: " + primary_attributes_ [cnt].AttriName + ", value: " + primary_attributes_ [cnt].TotalValue);
				}

				for (int vital_cnt = 0; vital_cnt < vitals_.Length; vital_cnt++) {
						Debug.Log ("vital name: " + ((VitalName)vital_cnt).ToString () + ", value: " + vitals_ [vital_cnt].TotalValue);
				}

				for (int skill_cnt = 0; skill_cnt < skills_.Length; skill_cnt++) {
			
						Debug.Log ("skill name: " + ((SkillName)skill_cnt).ToString () + ", value: " + skills_ [skill_cnt].TotalValue);
				}
		}
}
