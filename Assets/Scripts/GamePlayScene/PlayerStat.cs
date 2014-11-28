using UnityEngine;
using System.Collections;

public class PlayerStat : CharacterStat
{
		private Hashtable ht = new Hashtable ();
		private float max = 12;
		private float curr = 12;
		// Use this for initialization
		void Start ()
		{
				Debug.Log ("PlayerStat -> start() -> init attributes");
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyUp (KeyCode.A)) {
//						ht ["maxHealth"] = max;
//						ht ["currHealth"] = curr;
//						NotificationCenter.DefaultCenter ().PostNotification (this, GameDatabase.ResizeMobHealthBar, ht);
				}
		}	

		private void Attack ()
		{

		}

		private void SetAttackTarget (Transform target)
		{

		}
}
