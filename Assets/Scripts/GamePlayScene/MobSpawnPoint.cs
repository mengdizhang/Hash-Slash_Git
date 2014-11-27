using UnityEngine;
using System.Collections;
using System.IO;
using System; //to access to the enum
public class MobSpawnPoint : MonoBehaviour
{
		private Transform mobContainer;
		private string ZomBunnyMobPrefabPath;
		private string HellephantMobPrefabPath;
		private string ZomBearMobPrefabPath;
		//private MobCharacterStat[] stats;
		// Use this for initialization
		void Start ()
		{
				Debug.Log ("MobSpawnPoint -> Start() -> Start to create mobs to the scene.....");
				mobContainer = GameObject.Find (GameDatabase.PrefabPath.Mobs.ToString ()).transform;
				ZomBunnyMobPrefabPath = GameDatabase.PrefabPath.Prefabs.ToString () 
						+ Path.AltDirectorySeparatorChar 
						+ GameDatabase.PrefabPath.Mobs.ToString () + 
						Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.ZomBunny.ToString ();
				HellephantMobPrefabPath = GameDatabase.PrefabPath.Prefabs.ToString () + 
						Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Mobs.ToString () + 
						Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Hellephant.ToString ();
				ZomBearMobPrefabPath = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar +
						GameDatabase.PrefabPath.Mobs.ToString () + 
						Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.ZomBear.ToString ();

				InitHellephantMob ();
				InitZomBunnyMob ();
				InitZomBearMob ();

				//init mob stat instances


				//init mob  health bar
				string mobHealthBarPath = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + 
						GameDatabase.PrefabPath.Mobs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.MobHealthBar.ToString ();
				GameObject mobHealthBar = GameObject.Instantiate (Resources.Load (mobHealthBarPath)) as GameObject;
				mobHealthBar.isStatic = true;
				mobHealthBar.transform.parent = mobContainer;
		}
	
		// Update is called once per frame
		void Update ()
		{

		}

		private void InitHellephantMob ()
		{
				//init mobs 
				GameObject hellephant = GameObject.Instantiate (Resources.Load (HellephantMobPrefabPath)) as GameObject;
				//hellephant.tag = GameDatabase.MobTag;
				hellephant.transform.parent = mobContainer;
				
		}

		private void InitZomBunnyMob ()
		{
				//init mobs 
				GameObject hellephant = GameObject.Instantiate (Resources.Load (ZomBunnyMobPrefabPath)) as GameObject;
				//hellephant.tag = GameDatabase.MobTag; already setup tag Mob IN INSPECTOR
				hellephant.transform.parent = mobContainer;
		}

		private void InitZomBearMob ()
		{
				//init mobs 
				GameObject hellephant = GameObject.Instantiate (Resources.Load (ZomBearMobPrefabPath)) as GameObject;
				//hellephant.tag = GameDatabase.MobTag; already setup tag Mob IN INSPECTOR
				hellephant.transform.parent = mobContainer;
		}
}
