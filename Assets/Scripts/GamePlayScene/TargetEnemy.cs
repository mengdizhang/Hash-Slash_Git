using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class TargetEnemy : MonoBehaviour
{
		///tartgetting enemeies
		public GameObject target;
		public List<Transform> targets;
		public GameObject[] enemies;
		public Transform selectedTarget;
		private Hashtable ht = new Hashtable ();
		// Use this for initialization
		void Start ()
		{
				///tartgetting enemeies
				selectedTarget = null;
				enemies = GameObject.FindGameObjectsWithTag (GameDatabase.MobTag);
				targets = new List<Transform> ();
				foreach (GameObject go in enemies) {
						targets.Add (go.transform);
				}
		}
	
		/// Update is called once per frame
		void Update ()
		{
				/*target enemy*/
				if (Input.GetKeyUp (KeyCode.Tab)) {
						UpdatTarget ();
				}
		}

	
		public void sort_targets_by_dist ()
		{
				targets.Sort (delegate(Transform t1, Transform t2) {
						return Vector3.Distance (t1.position, transform.position).CompareTo 
				(Vector3.Distance (t2.position, transform.position));
				});
				selectedTarget = targets [0];
				//selectedTarget.renderer.material.color = Color.red;
		}

		private void  UpdatTarget ()
		{
				if (selectedTarget == null) {
						sort_targets_by_dist ();
						NotificationCenter.DefaultCenter ().PostNotification (this, GameDatabase.ShowHealthBar);
				}
				int index = targets.IndexOf (selectedTarget);
				if (index < targets.Count - 1) {
				
						index++;
				} else
						index = 0;
				//								selectedTarget.renderer.material.color = Color.blue;
				selectedTarget.FindChild ("Name").GetComponent<MeshRenderer> ().enabled = false;
				selectedTarget = targets [index];
				//selectedTarget.renderer.material.color = Color.red;
				selectedTarget.FindChild ("Name").GetComponent<MeshRenderer> ().enabled = true;
				target = selectedTarget.gameObject;
				ht ["maxHealth"] = ht ["currHealth"] = (float)target.GetComponent<CharacterStat> ().GetVital ((int)VitalName.Health).TotalValue;
				NotificationCenter.DefaultCenter ().PostNotification (this, GameDatabase.ResizeMobHealthBar, ht);
		}
}
