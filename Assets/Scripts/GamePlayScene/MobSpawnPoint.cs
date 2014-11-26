using UnityEngine;
using System.Collections;
using System.IO;
public class MobSpawnPoint : MonoBehaviour
{
		private Transform mobContainer;
		// Use this for initialization
		void Start ()
		{
				Debug.Log ("MobSpawnPoint -> Start() -> Start to create mobs to the scene.....");
				mobContainer = GameObject.Find (GameDatabase.PrefabPath.Mobs.ToString ()).transform;

				//init mobs 
				string path = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Mobs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Hellephant.ToString ();
				GameObject hellephant = GameObject.Instantiate (Resources.Load (path)) as GameObject;
				hellephant.tag = GameDatabase.MobTag;
				hellephant.transform.parent = mobContainer;
			
				//init mob  health bar
				string mobHealthBarPath = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Mobs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.MobHealthBar.ToString ();
				GameObject mobHealthBar = GameObject.Instantiate (Resources.Load (mobHealthBarPath)) as GameObject;
				mobHealthBar.isStatic = true;
				mobHealthBar.transform.parent = mobContainer;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
