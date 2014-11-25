/**
 * @file
 * @brief 
 * 
 * 
 * 
 * @author 
 * @date 
 * 
 */

// .NET includes
using System;
// Unity includes
using UnityEngine;

using System.IO;
/**
 * @brief 
 */
public class GameDirector : MonoBehaviour
{		
		void Start ()
		{
				InitializeScene ();
		}

		private void InitializeScene ()
		{
				#region init container gos
				//DynamicObjs container go
				GameObject dynamicObjs = new GameObject ();
				dynamicObjs.name = "DynamicObjs";
				dynamicObjs.isStatic = true;
				//dynamicObjs.tag = "DynamicObjs";
				dynamicObjs.transform.parent = transform;
		
				//envirs container go
				GameObject envirs = new GameObject ();
				envirs.name = GameDatabase.PrefabPath.Environments.ToString ();
				envirs.isStatic = true;
				//envirs.tag = GameDatabase.PrefabPath.Environments.ToString ();
				envirs.transform.parent = dynamicObjs.transform;
				////setup floor
				//GameObject floor  = GameObject.CreatePrimitive(PrimitiveType.Quad);
				//GameObject.Destroy(floor.GetComponent<MeshRenderer>());
				//floor.name = "Floor";
				//floor.transform.localScale.Set(100f, 100f, 1);
				//floor.layer = LayerMask.NameToLayer("Floor");
				//floor.transform.parent = envirs.transform;
				////setup BAKGROUND MUSIC
				////GameObject bgMusic = new GameObject("bgMusic");
				////bgMusic.AddComponent<AudioSource>();
				////bgMusic.GetComponent<AudioSource>().loop = true;
				////bgMusic.GetComponent<AudioSource>().clip
				//Masters container go
				GameObject Mobs = new GameObject ();
				Mobs.name = GameDatabase.PrefabPath.Mobs.ToString ();
				Mobs.isStatic = true;
				//character.tag = GameDatabase.PrefabPath.Masters.ToString ();
				Mobs.transform.parent = dynamicObjs.transform;
		
				//players container go
				GameObject players = new GameObject ();
				players.name = GameDatabase.PrefabPath.Players.ToString ();
				players.isStatic = true;
				//players.tag = GameDatabase.PrefabPath.Players.ToString ();
				players.transform.parent = dynamicObjs.transform;
		
				//npc container go
				GameObject npc = new GameObject ();
				npc.name = GameDatabase.PrefabPath.NPCs.ToString ();
				npc.isStatic = true;
				//npc.tag = GameDatabase.PrefabPath.NPCs.ToString ();
				npc.transform.parent = dynamicObjs.transform;
		
				//spwan point  container go
				GameObject spawnPoints = new GameObject ();
				spawnPoints.name = GameDatabase.PrefabPath.SpawanPoints.ToString ();
				spawnPoints.isStatic = true;
				//spawnPoints.tag = GameDatabase.PrefabPath.SpawanPoints.ToString ();
				spawnPoints.transform.parent = dynamicObjs.transform;
				#endregion
		
				#region init gos instiated from prefabs and append them to right container
				string path = "";
				//init playerSpwanPoint
				path = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.SpawanPoints.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.PlayerSpwanPoint.ToString ();
				GameObject playerSpwanPoint = GameObject.Instantiate (Resources.Load (path)) as GameObject;
				playerSpwanPoint.transform.parent = spawnPoints.transform;
		
//				//init monsterSpwanPoint
//				path = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.SpawanPoints.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.MonsterSpwanPoint.ToString ();
//				GameObject monsterSpwanPoint = GameObject.Instantiate (Resources.Load (path)) as GameObject;
//				monsterSpwanPoint.transform.parent = spawnPoints.transform;
		
				//init envir  to scene
				path = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Environments.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Envir.ToString ();
				GameObject envir = GameObject.Instantiate (Resources.Load (path)) as GameObject;
				envir.transform.parent = envirs.transform;
		
		
				//init lights  to scene
				path = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Environments.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Lights.ToString ();
				GameObject lights = GameObject.Instantiate (Resources.Load (path)) as GameObject;
				lights.transform.parent = envirs.transform;

				//init player
				path = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Players.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.WhitePlayer.ToString ();
				GameObject whitePlayer = GameObject.Instantiate (Resources.Load (path)) as GameObject;
				whitePlayer.tag = GameDatabase.PrefabPath.WhitePlayer.ToString ();
				whitePlayer.transform.parent = players.transform;
		
				//init player health bar
				string playerHealthBarPath = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Players.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.PlayerHealthBar.ToString ();
				GameObject playerHealthBar = GameObject.Instantiate (Resources.Load (playerHealthBarPath)) as GameObject;
				playerHealthBar.isStatic = true;
				//playerHealthBar.tag = GameDatabase.PrefabPath.PlayerHealthBar.ToString ();
				playerHealthBar.transform.parent = players.transform;

				//init mob  health bar
				string mobHealthBarPath = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Mobs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.MobHealthBar.ToString ();
				GameObject mobHealthBar = GameObject.Instantiate (Resources.Load (mobHealthBarPath)) as GameObject;
				mobHealthBar.isStatic = true;
				//playerHealthBar.tag = GameDatabase.PrefabPath.PlayerHealthBar.ToString ();
				mobHealthBar.transform.parent = Mobs.transform;
		
//				// Calculate the initial offset.
				CameraFollow scr = Camera.main.gameObject.AddComponent<CameraFollow> ();
				//scr.target = whitePlayer.transform;
				#endregion

				
		}
}

/* EOF */
