using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
		public float maxHealth = 100f;
		public float currHealth = 100f;
		
		public float maxBarLen;
		public float currBarLen;
		private GUITexture healthBar;
		private PlayerCharacterStat playerStat;
		public bool isPlayerHealthBar = false;

		// Use this for initialization
		void Start ()
		{
				if (isPlayerHealthBar)
						Debug.Log ("PlayerHealthBar Start()");
				else
						Debug.Log ("MobHealthBar Start()");

					
				healthBar = gameObject.GetComponent<GUITexture> ();
				currBarLen = maxBarLen = healthBar.pixelInset.width;

				//playerStat = GameDatabase.Get<PlayerCharacterBean> (GameDatabase.PlayerCharacterBean);
				playerStat = new PlayerCharacterStat ();
		}
	
		// Update is called once per frame
		void Update ()
		{
//				if (isPlayerHealthBar) {
//						GameDatabase.ht ["currHealth"] = currHealth;
//						GameDatabase.ht ["maxHealth"] = maxHealth;
//						NotificationCenter.DefaultCenter ().PostNotification (this, "ResizePlayerHealthBar", GameDatabase.ht); 
//				}
		}

		void OnEnable ()
		{
				Debug.Log (" isPlayerHealthBar = " + isPlayerHealthBar + "go name is " + gameObject.name);
				if (isPlayerHealthBar) {
						Debug.Log ("PlayerHealthBar OnEnable() AddObserver");
						NotificationCenter.DefaultCenter ().AddObserver (this, "ResizePlayerHealthBar");
				} else {
						Debug.Log ("MobHealthBar OnEnable() AddObserver");
						NotificationCenter.DefaultCenter ().AddObserver (this, "ResizeMobHealthBar");
				}

		}
	
		void OnDisable ()
		{				
				if (isPlayerHealthBar) {
						Debug.Log ("PlayerHealthBar OnDisable() RemoveObserver");
						NotificationCenter.DefaultCenter ().RemoveObserver (this, "ResizePlayerHealthBar");
				} else {
						Debug.Log ("MobHealthBar OnDisable() RemoveObserver");
						NotificationCenter.DefaultCenter ().RemoveObserver (this, "ResizeMobHealthBar");
				}
		}

		void ResizePlayerHealthBar (Notification notification)
		{
				Hashtable ht = (Hashtable)notification.data;
				Debug.Log ("curr health = " + (float)ht ["currHealth"] + ", maxHealth = " + (float)ht ["maxHealth"]);
				float currHealth = (float)ht ["currHealth"];
				float maxHealth = (float)ht ["maxHealth"];
				currBarLen = (currHealth / maxHealth) * maxBarLen;
				healthBar.pixelInset = new Rect (healthBar.pixelInset.x, healthBar.pixelInset.y, currBarLen, healthBar.pixelInset.height);
		}

		void ResizeMobHealthBar (Notification notification)
		{
				Hashtable ht = (Hashtable)notification.data;
				Debug.Log ("curr health = " + (float)ht ["currHealth"] + ", maxHealth = " + (float)ht ["maxHealth"]);
				float currHealth = (float)ht ["currHealth"];
				float maxHealth = (float)ht ["maxHealth"];
				currBarLen = (currHealth / maxHealth) * maxBarLen;
				healthBar.pixelInset = new Rect (healthBar.pixelInset.x, healthBar.pixelInset.y, currBarLen, healthBar.pixelInset.height);
		}

}
