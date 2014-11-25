using UnityEngine;
using System.Collections;

public class PlayerHealthBar : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{

		}
	
		// Update is called once per frame
		void Update ()
		{
				Debug.Log ("PlayerHealthBar Update()");
		}

		void OnEnable ()
		{
				Debug.Log ("PlayerHealthBar - onEnable() - AddObserver");
				NotificationCenter.DefaultCenter ().AddObserver (this, "ResizeHealthBar");
		}
	
		void OnDisable ()
		{
				Debug.Log ("PlayerHealthBar onDisable() RemoveObserver");
				NotificationCenter.DefaultCenter ().RemoveObserver (this, "ResizeHealthBar");
		}
	
		void ResizeHealthBar (Notification notification)
		{
				Hashtable ht = (Hashtable)notification.data;
				Debug.Log ("PlayerHealthBar ResizeHealthBar()");
				Debug.Log ("curr health = " + (int)ht ["currHealth"] + ", maxHealth = " + (int)ht ["maxHealth"]);
		}
}
