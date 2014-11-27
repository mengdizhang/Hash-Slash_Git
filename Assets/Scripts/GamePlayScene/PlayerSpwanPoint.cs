using UnityEngine;
using System.Collections;
using System.IO;
public class PlayerSpwanPoint : MonoBehaviour
{
		private Transform playerContainer;
		// Use this for initialization
		void Start ()
		{
				Debug.Log ("PlayerSpwanPoint -> start() -> start create player to the scene....");

				playerContainer = GameObject.Find (GameDatabase.PrefabPath.Players.ToString ()).transform;

				//init player
				string path = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Players.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.WhitePlayer.ToString ();
				GameObject whitePlayer = GameObject.Instantiate (Resources.Load (path)) as GameObject;
				whitePlayer.tag = GameDatabase.PrefabPath.WhitePlayer.ToString ();
				whitePlayer.transform.parent = playerContainer;
		
				//Calculate the initial offset.	
				CameraFollow cf = Camera.main.gameObject.AddComponent<CameraFollow> ();

				//init player health bar
				string playerHealthBarPath = GameDatabase.PrefabPath.Prefabs.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.Players.ToString () + Path.AltDirectorySeparatorChar + GameDatabase.PrefabPath.PlayerHealthBar.ToString ();
				GameObject playerHealthBar = GameObject.Instantiate (Resources.Load (playerHealthBarPath)) as GameObject;
				playerHealthBar.isStatic = true;
				//playerHealthBar.tag = GameDatabase.PrefabPath.PlayerHealthBar.ToString ();
				playerHealthBar.transform.parent = playerContainer;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
