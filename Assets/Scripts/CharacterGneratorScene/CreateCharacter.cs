/**
 * @file
 * @brief 
 * 
 * 
 * 
 * @author Jackie Zhang
 * @date  27/11/2014 - 3:18 PM
 * 
 */

// .NET includes
using System;
// Unity includes
using UnityEngine;
// Custom includes

/**
 * @brief 
 */
public class CreateCharacter : MonoBehaviour
{
		private CharacterStat playerCharacterBean;
		
		private int MAX_POINTS;
		private int points_left_;
	
		private Rect name_label_;
		private Rect name_text_area_;
	
		private Rect create_bnt_rect_;
	
		private Rect point_label_;
		private Rect point_text_area_;
		private int Each_Attri_Init_Point;
	
		private Rect[] attri_name_rects_;
		private Rect[] attri_text_area_rects_;
		private Rect[] attri_minus_button_rects_;
		private Rect[] attri_add_button_rects_;
	
		private Rect[] vital_name_rects_;
		private Rect[] vital_text_area_rects_;
	
	
		private Rect[] skill_text_area_rects_;
		private Rect[] skill_name_rects_;
	
		private int cnt;
		private float base_left_;
		private float base_height_;
		private float rect_width_;
		private float rect_height_;
		void Awake ()
		{
				Debug.Log ("CreateCharacter -> Awake()");
		}
		void Start ()
		{
				//GameDatabase.logFlag = false;
				Debug.Log ("CreateCharacter -> Start()");
				Init ();
		}
	
		void OnGUI ()
		{
				//GUI.skin = my_skin_; // this can change all the buttons panels so good to change as batch need call every frame
				//GUI.Label(name_label_, "Name", my_style_); 

				//name
				GUI.Label (name_label_, "Name");
				playerCharacterBean.Name = GUI.TextArea (name_text_area_, playerCharacterBean.Name);
		
				//points left
				//GUI.Label(point_label_, "Point Left", my_style_);
				GUI.Label (point_label_, "Point Left");
				GUI.Label (point_text_area_, points_left_.ToString ());
		
				//init create bnt
				if (GUI.Button (create_bnt_rect_, "Create"/*, create_bnt_style_*/)) {
						onCreateCharacterBntClicked ();
						//dispatcher.Dispatch (ViewEvent.CREATE_CHARACTER_BNT_CLICKED_EVENT);
				}
		
				//attri
				for (cnt = 0; cnt < attri_name_rects_.Length; cnt++) {
						//GUI.Label(attri_name_rects_[cnt], ((AttributeName)cnt).ToString() + ": ",my_style_);
						GUI.Label (attri_name_rects_ [cnt], ((CharacterAttributeName)cnt).ToString () + ": ");
						GUI.Label (attri_text_area_rects_ [cnt], playerCharacterBean.GetPrimaryAttribute (cnt).TotalValue.ToString ());
			
						if (GUI.Button (attri_add_button_rects_ [cnt], "+")) {
								onIncreaseAttriBntClicked (cnt);
						}
			
						if (GUI.Button (attri_minus_button_rects_ [cnt], "-")) {
								onDecreaseAttriBntClicked (cnt);
						}
				}
		
				///vital
				for (cnt = 0; cnt < vital_name_rects_.Length; cnt++) {
						//GUI.Label(vital_name_rects_[cnt], ((VitalName)cnt).ToString(), my_style_);
						GUI.Label (vital_name_rects_ [cnt], ((VitalName)cnt).ToString ()); //will use default gui skin
						GUI.Label (vital_text_area_rects_ [cnt], playerCharacterBean.GetVital (cnt).TotalValue.ToString ());
				}
		
				//skills
				for (cnt = 0; cnt < skill_name_rects_.Length; cnt++) {
						//GUI.Label(skill_name_rects_[cnt], ((SkillName)cnt).ToString(),my_style_);
						GUI.Label (skill_name_rects_ [cnt], ((SkillName)cnt).ToString ());
						GUI.Label (skill_text_area_rects_ [cnt], playerCharacterBean.GetSkill (cnt).TotalValue.ToString ());
				}
		}
	
		private void Init ()
		{
		
				Debug.Log (" init() called ");
				playerCharacterBean = this.GetComponent<CharacterStat> ();
		
				rect_width_ = 50f;
				rect_height_ = 25f;
				base_left_ = (Screen.width - base_left_ - 11 * rect_width_) / 2; //make it center;
				base_height_ = 10f;
		
				MAX_POINTS = 50;
				Each_Attri_Init_Point = 5;
		
				points_left_ = MAX_POINTS - Enum.GetValues (typeof(CharacterAttributeName)).Length * Each_Attri_Init_Point;
		
				//initial name
				name_label_ = new Rect (base_left_, base_height_, 2 * rect_width_, rect_height_);
				name_text_area_ = new Rect (base_left_ + 2 * rect_width_, base_height_, 2 * rect_width_, rect_height_);
		
				//initial points left
				point_label_ = new Rect (base_left_ + 5 * rect_width_, base_height_, 2 * rect_width_, rect_height_);
				point_text_area_ = new Rect (base_left_ + 8 * rect_width_, base_height_, rect_width_, rect_height_);
		
				//initial create button
				create_bnt_rect_ = new Rect (base_left_ + 10 * rect_width_, base_height_, 2 * rect_width_, rect_height_);
		
				float new_height = rect_height_ + base_height_; //to make it easy ti type just use a new variable
		
				//initial attri
				attri_name_rects_ = new Rect[Enum.GetValues (typeof(CharacterAttributeName)).Length];
				attri_text_area_rects_ = new Rect[attri_name_rects_.Length];
				attri_minus_button_rects_ = new Rect[attri_name_rects_.Length];
				attri_add_button_rects_ = new Rect[attri_name_rects_.Length];
				for (cnt = 0; cnt < attri_name_rects_.Length; cnt++) {
						attri_name_rects_ [cnt].Set (base_left_, new_height + cnt * new_height, 2 * rect_width_, rect_height_);
						attri_text_area_rects_ [cnt].Set (base_left_ + 2 * rect_width_, new_height + cnt * new_height, rect_width_, rect_height_);
						attri_add_button_rects_ [cnt].Set (base_left_ + 3 * rect_width_, new_height + cnt * new_height, rect_height_, rect_height_);
						attri_minus_button_rects_ [cnt].Set (base_left_ + 3 * rect_width_ + rect_height_, new_height + cnt * new_height, rect_height_, rect_height_);
				}
				float new_left_off = 5 * rect_width_;
		
				//initial vital
				vital_text_area_rects_ = new Rect[Enum.GetValues (typeof(VitalName)).Length];
				vital_name_rects_ = new Rect[Enum.GetValues (typeof(VitalName)).Length];
				for (cnt = 0; cnt < vital_name_rects_.Length; cnt++) {
						vital_name_rects_ [cnt].Set (base_left_ + new_left_off, new_height + cnt * new_height, 2 * rect_width_, rect_height_);
						vital_text_area_rects_ [cnt].Set (base_left_ + new_left_off + 3 * rect_width_, new_height + cnt * new_height, rect_width_, rect_height_);
				}
				new_left_off *= 2; //move to right 
		
				//initial skills
				skill_text_area_rects_ = new Rect[Enum.GetValues (typeof(SkillName)).Length];
				skill_name_rects_ = new Rect[Enum.GetValues (typeof(SkillName)).Length];
				for (cnt = 0; cnt < skill_name_rects_.Length; cnt++) {
						skill_name_rects_ [cnt].Set (base_left_ + new_left_off, new_height + cnt * new_height, 2 * rect_width_, rect_height_);
						skill_text_area_rects_ [cnt].Set (base_left_ + new_left_off + 3 * rect_width_, new_height + cnt * new_height, rect_width_, rect_height_);
				}
		}
	
	
		private void onIncreaseAttriBntClicked (int data)
		{
				Debug.Log ("CreateCharacter -> onIncreaseAttriBntClicked() called");
				if (points_left_ > 0) {
						playerCharacterBean.GetPrimaryAttribute (data).BaseValue++;
						points_left_--;
						playerCharacterBean.UpdateStats (); //update all stats
				}
		}
		private void onDecreaseAttriBntClicked (int data)
		{
				Debug.Log ("CreateCharacter -> onDecreaseAttriBntClicked() called");
				if (points_left_ < MAX_POINTS && playerCharacterBean.GetPrimaryAttribute (data).BaseValue > 0) {
						playerCharacterBean.GetPrimaryAttribute (data).BaseValue--;
						points_left_++;
						playerCharacterBean.UpdateStats (); //update all stats
				}
		}

		private void InitEnterNeweScene ()
		{
				GetComponent<StartGame> ().enabled = false;
				this.enabled = false;
				gameObject.AddComponent<GamePlaying> ();
		}
		private void onCreateCharacterBntClicked ()
		{
				Debug.Log ("CreateCharacter -> onCreateCharacterBntClicked() called");
				InitEnterNeweScene ();
				Application.LoadLevel (GameDatabase.SceneName.StartGameScene1.ToString ());
		}
}

/* EOF */
