using UnityEngine;
using System.Collections;

public class PlayerStat : MonoBehaviour
{
		private PlayerCharacterBean playerBean = GameDatabase.Get<PlayerCharacterBean> (GameDatabase.PlayerCharacterBean);
		// Use this for initialization
		void Start ()
		{
				Debug.Log ("PlayerStat start()");
				if (playerBean == null)
						Debug.Log ("player instance is null");
				else
						Debug.Log ("player instance is there" + playerBean.ToString ());
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
