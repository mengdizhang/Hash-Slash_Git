using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
		public float maxHealth;
		public float currHealth;
		
		public float maxBarLen;
		public float currBarLen;
		private GUITexture healthBar;
		//private PlayerCharacterStat playerStat;
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
				//playerStat = new PlayerCharacterStat ();
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
				NotificationCenter.DefaultCenter ().AddObserver (this, "ShowHealthBar");
				NotificationCenter.DefaultCenter ().AddObserver (this, "HideHealthBar");
				NotificationCenter.DefaultCenter ().AddObserver (this, "InitHealthBarPoints");
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
				NotificationCenter.DefaultCenter ().RemoveObserver (this, "ShowHealthBar");
				NotificationCenter.DefaultCenter ().RemoveObserver (this, "HideHealthBar");
				NotificationCenter.DefaultCenter ().AddObserver (this, "InitHealthBarPoints");
		}

		void ResizePlayerHealthBar (Notification notification)
		{
				Debug.Log ("ResizePlayerHealthBar");
				currBarLen = (currHealth / maxHealth) * maxBarLen;
				healthBar.pixelInset = new Rect (healthBar.pixelInset.x, healthBar.pixelInset.y, currBarLen, healthBar.pixelInset.height);
		}

		void ResizeMobHealthBar (Notification notification)
		{	
				Debug.Log ("ResizeMobHealthBar");
				currBarLen = (currHealth / maxHealth) * maxBarLen;
				healthBar.pixelInset = new Rect (healthBar.pixelInset.x, healthBar.pixelInset.y, currBarLen, healthBar.pixelInset.height);
		}

		void ShowHealthBar (Notification notification)
		{
				healthBar.enabled = true;
		}

		void HideHealthBar (Notification notification)
		{
				healthBar.enabled = false;
		}

}
