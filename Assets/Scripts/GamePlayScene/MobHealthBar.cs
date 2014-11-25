using UnityEngine;
using System.Collections;

public class MobHealthBar : MonoBehaviour
{
		private int maxBarLen = 0;
		private int currBarLen = 0;

		// Use this for initialization
		void Start ()
		{
				Debug.Log ("MobHealthBar Start()");
		}
	
		// Update is called once per frame
		void Update ()
		{
				Debug.Log ("MobHealthBar Update()");
//				GameDatabase.ht ["currHealth"] = 80;
//				GameDatabase.ht ["maxHealth"] = 100;
//				NotificationCenter.DefaultCenter ().PostNotification (this, "ResizeHealthBar", GameDatabase.ht);
		}

		void OnEnable ()
		{
				Debug.Log ("MobHealthBar - onEnable() - AddObserver");
				NotificationCenter.DefaultCenter ().AddObserver (this, "ResizeHealthBar");
		}
	
		void OnDisable ()
		{
				Debug.Log ("MobHealthBar onDisable() RemoveObserver");
				NotificationCenter.DefaultCenter ().RemoveObserver (this, "ResizeHealthBar");
		}

		void ResizeHealthBar (Notification notification)
		{
				Hashtable ht = (Hashtable)notification.data;
				Debug.Log ("MobHealthBar ResizeHealthBar()");
				Debug.Log ("curr health = " + (int)ht ["currHealth"] + ", maxHealth = " + (int)ht ["maxHealth"]);
		}
}
