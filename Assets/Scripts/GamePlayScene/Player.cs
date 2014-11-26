using UnityEngine;
using System.Collections.Generic;
public class Player : MonoBehaviour
{
	/*health bar*/
	public float max_health_;
	public float current_health_;
	public float health_bar_len_;
	public float HEALTH_BAR_LEN;
	public Rect rect;

	/*attack*/
	public GameObject target_;
	public float attack_timer_;
	public float cool_down_;

	/*tartgetting enemeies*/
	public List<Transform> targets_;
	public GameObject[] enemies_;
	public Transform select_target_;

	// Use this for initialization
	void Start()
	{
		/*health bar*/
		max_health_ = 100;
		current_health_ = 100;
		HEALTH_BAR_LEN = health_bar_len_ = Screen.width / 2;
		rect = new Rect(10, 40, health_bar_len_, 20);

		/*attack*/
		attack_timer_ = 0; //setup to 0 to make the first attack avaiable
		cool_down_ = 2.0f;

		/*tartgetting enemeies*/
		select_target_ = null;
		enemies_ = GameObject.FindGameObjectsWithTag("enemy1");
		targets_ = new List<Transform>();
		foreach (GameObject go in enemies_)
		{
			targets_.Add(go.transform);
		}
	}

	// Update is called once per frame
	void Update()
	{
		/*attack enemy*/
		if (attack_timer_ > 0)
			attack_timer_ -= Time.deltaTime;
		else
			attack_timer_ = 0;

		if (Input.GetKeyUp(KeyCode.A) && attack_timer_ == 0)
		{
			attack(); //attack enemy
			attack_timer_ = cool_down_; //reset cooldown
		}

		/*target enemy*/
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			if (select_target_ == null)
			{
				sort_targets_by_dist();
			}
			else
			{
				int index = targets_.IndexOf(select_target_);
				if (index < targets_.Count-1)
				{

					index++;
				}
				else
					index = 0;
				select_target_.renderer.material.color = Color.blue;
				select_target_ = targets_[index];
				select_target_.renderer.material.color = Color.red;
			}

			target_ = select_target_.gameObject;
		}
	}

	void OnGUI()
	{
		update_current_health(0);
		rect.width = health_bar_len_;
		GUI.Box(rect, "Player health bar: " + current_health_ + " / " + max_health_);
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
		if (dist < 2.5f && dot > 0)
		{
			Enemy enemy = (Enemy)target_.GetComponent("Enemy");
			enemy.update_current_health(-10);
		}
	}

	/*tartgetting enemeies*/
	public void add_tartget_enemy(Transform enemy)
	{
		targets_.Add(enemy);
	}

	public void sort_targets_by_dist()
	{
		targets_.Sort(delegate(Transform t1, Transform t2)
		{
			return Vector3.Distance(t1.position, transform.position).CompareTo(Vector3.Distance(t2.position, transform.position));
		});
		select_target_ = targets_[0];
		select_target_.renderer.material.color = Color.red;
	}
}
