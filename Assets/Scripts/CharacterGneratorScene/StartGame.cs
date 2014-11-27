/**
 * @file
 * @brief 
 * 
 * first scene script
 * 
 * @author Jackie
 * @date  3:21 PM 24/11/2014
 * 
 */

// .NET includes
using System;
// Unity includes
using UnityEngine;
// Custom includes
using System.IO;

/**
 * @brief 
 */
public class StartGame:MonoBehaviour
{
		void Start ()
		{
				//init player
				gameObject.AddComponent<CreateCharacter> ();
				gameObject.AddComponent<CharacterStat> ();
				DontDestroyOnLoad (this);
		}
} 
	
/* EOF */
