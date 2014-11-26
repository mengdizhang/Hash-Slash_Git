using UnityEngine;
using System.Collections;

public class PlayerStat : MonoBehaviour
{

		private MobCharacterStat mobStat;

		// Use this for initialization
		void Start ()
		{
				mobStat = GameDatabase.Add<MobCharacterStat> (GameDatabase.MobCharacterBean, new MobCharacterStat ());
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
