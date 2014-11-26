using UnityEngine;
using System.Collections.Generic;

public class TargetEnemy : MonoBehaviour
{
		///tartgetting enemeies
		public GameObject target;
		public List<Transform> targets;
		public GameObject[] enemies;
		public Transform selectedTarget;

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
						if (selectedTarget == null) {
								sort_targets_by_dist ();
						} else {
								int index = targets.IndexOf (selectedTarget);
								if (index < targets.Count - 1) {
					
										index++;
								} else
										index = 0;
//								selectedTarget.renderer.material.color = Color.blue;
								selectedTarget = targets [index];
								selectedTarget.renderer.material.color = Color.red;
						}
			
						target = selectedTarget.gameObject;
				}
		}

		///tartgetting enemeies
		public void add_tartget_enemy (Transform enemy)
		{
				targets.Add (enemy);
		}
	
		public void sort_targets_by_dist ()
		{
				targets.Sort (delegate(Transform t1, Transform t2) {
						return Vector3.Distance (t1.position, transform.position).CompareTo (Vector3.Distance (t2.position, transform.position));
				});
				selectedTarget = targets [0];
				//selectedTarget.renderer.material.color = Color.red;
		}
}
