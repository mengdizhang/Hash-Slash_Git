using UnityEngine;
using System.Collections;

public class CharacterAttribute : BaseStat
{
	private CharacterAttributeName attri_name_;

	public CharacterAttribute()
	{
		base_value_ = 5;
		attri_name_ = CharacterAttributeName.Might;
		Exp2Nextlevel = 50;
		LevelModifier = 1.05f;
	}

	public CharacterAttributeName AttriName
	{
		get { return attri_name_; }
		set { attri_name_ = value; }
	}
}

public enum CharacterAttributeName
{
	Might, //力量
	Constitution, //体质
	Nimbleness, //敏捷
	Speed, //速度
	Concentration, //专注
	Willpower, //意志力
	Charisma//魅力
}