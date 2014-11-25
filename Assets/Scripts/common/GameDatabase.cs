using UnityEngine;
using System.Collections;

public class GameDatabase
{
		
		public static Hashtable ht = new Hashtable ();
		public static string GameDirector = "GameDirector";
		public static string GameMaster = "GameMaster";
		public static string Hellephant = "Hellephant";
		public static string PlayerCharacterBean = "PlayerCharacterBean";

		//BaseStatBean initial values
		public static int base_value_ = 0;
		public static int buff_value_ = 0;
		public static int total_value_ = base_value_ + buff_value_;
		public static int exp2next_level_ = 100;
		public static float exp2next_level_modifier_ = 1.1f;


		//VitalBean initial values
		public static int Vitalbean_Exp2Nextlevel = 25;
		public static float Vitalbean_LevelModifier = 1.1f;

		//SkillBean initial values
		public static int SkillBean_Exp2Nextlevel = 25;
		public static float SkillBean_LevelModifier = 1.1f;

		//control debug log 
		public static bool logFlag = true;

		//all prefabs paths
		public enum PrefabPath
		{
				/// <summary>
				///  /Resources/Prefabs/Character/ZomBear
				///  a prefab named of ZomBear is stored in /Resources/Prefabs/Character
				/// </summary>
		
				Prefabs,/*dir path : /Resources/Prefabs*/
		
				Mobs, /* dir path : /Resources/Prefabs/Character */
				MobHealthBar, 
				Hellephant,/* /Resources/Prefabs/Character/Hellephant */
				ZomBear,/* /Resources/Prefabs/Character/ZomBear */
		
				Environments,/* dir path : /Resources/Prefabs/Environment */
				Envir,/* /Resources/Prefabs/Environment/Envir*/
				Lights,/* /Resources/Prefabs/Environment/Lights*/
		
				NPCs,/* dir path : /Resources/Prefabs/NPC */
		
				Players, /* dir path : /Resources/Prefabs/Player */
				PlayerHealthBar,  
				WhitePlayer,
		
				SpawanPoints, //sub1 Environment dir
				PlayerSpwanPoint,
				MonsterSpwanPoint,
		
				NONE 
		}
		public enum SceneName
		{
				StartGameScene1
		}

		public static T Get<T> (string name)
		{
				return (T)ht [name];
		}

		public static T Add<T> (string name, T t)
		{
				ht.Add (name, t);
				return t;
		}

		public void Remove (string name)
		{
				ht.Remove (name);
		}

		public static void Log (string str)
		{
				if (logFlag)
						Debug.Log (str);
		}

}
