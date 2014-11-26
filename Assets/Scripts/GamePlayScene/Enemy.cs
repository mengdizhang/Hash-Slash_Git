using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	/*enemy attack*/
	public GameObject target_;
	public float move_spped_;
	public float rotation_speed_;
	public float attack_timer_;
	public float cool_down_;
	public Player player_script_;
	public float ATTACK_DIST;

	/*enemy health*/
	public float max_health_;
	public float current_health_;
	public float health_bar_len_;
	public float HEALTH_BAR_LEN;
	public Rect rect;

	void Awake()
	{
		/*enemy attack*/
		move_spped_ = 1f;
		rotation_speed_ = 1f;
		attack_timer_ = 0; //setup to 0 to make the first attack avaiable
		cool_down_ = 2.0f;
		target_ = GameObject.FindGameObjectWithTag("Player");
		player_script_ = (Player)(target_.GetComponent("Player"));
		ATTACK_DIST = 2.5f;

		/*enemy health*/
		max_health_ = 100f;
		current_health_ = 100f;
		HEALTH_BAR_LEN = health_bar_len_ = Screen.width / 2;
		rect = new Rect(10f, 10f, health_bar_len_, 20f);

	}
	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		/*enemy attack*/
		//draw a line between the two objects
		Debug.DrawLine(target_.transform.position, transform.position, Color.yellow);
		//look at target
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target_.transform.position - transform.position), rotation_speed_ * Time.deltaTime);
		//move to target
		//if (Vector3.Distance(target_.transform.position, transform.position) > ATTACK_DIST)
		//transform.position += transform.forward * move_spped_ * Time.deltaTime;

		if (attack_timer_ > 0)
			attack_timer_ -= Time.deltaTime;
		else
			attack_timer_ = 0;

		if (attack_timer_ == 0)
		{
			attack(); //attack enemy
			attack_timer_ = cool_down_; //reset cooldown
		}
	}

	void OnGUI()
	{
		/*enemy health*/
		update_current_health(0);
		rect.width = health_bar_len_;
		GUI.Box(rect, "Enemy health bar: " + current_health_ + " / " + max_health_);
	}

	public void update_current_health(float num)
	{
		current_health_ += num;
		if (current_health_ < 0)
			current_health_ = 0;
		if (current_health_ > max_health_)
			current_health_ = max_health_;
		if (max_health_ < 1)
			max_health_ = 1;

		health_bar_len_ = HEALTH_BAR_LEN * (current_health_ / max_health_);
	}

	public void attack()
	{
		float dist = Vector3.Distance(target_.transform.position, transform.position);
		Vector3 dir_vct = (target_.transform.position - transform.position).normalized;
		float dot = Vector3.Dot(dir_vct, transform.forward);
		Debug.Log("distance = " + dist + " bot = " + dot);
		if (dist <= ATTACK_DIST && dot > 0)
		{
			player_script_.update_current_health(-10);
		}
	}
}