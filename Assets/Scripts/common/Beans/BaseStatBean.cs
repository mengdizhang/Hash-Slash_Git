/**
 * BaseStat.cs
 *
 * by Jackie Zhang on 23/10/2014
 *
 * This is the base class for all the figures that appear in the games 
 * such as in RPG in character system, when the character levels up, 
 * we can increase the @para base value of all the attribuites, at the same time 
 * @para total_value_ in these attributes will be updated in @method Update().
 * Vitals and skills's that are based on attribute will be also updated in  in @method Update().
 *
 * @para base_value_
 * the basic value of this stat
 *
 * @para buff_value_
 * emp added value such as the player get a positive effectivess 
 * his health bar will temperalily raise
 *
 * @para exp2next_level_
 * the total amounts of exp needed to raise this skill
 *
 * @para exp2next_level_modifier_
 * the modofier applied to the exp2next_level_
 * as character grows up, the exp he needs will increase at the same time.
 *
 * @para total_value_
 * the addition of @para base_value_ and @para buff_value_
 * it will be updated many times during the game when @para base_value_ or @para buff_value_ get changed
 * such as character level up, character get attacked.
 *
 * @method Update() 
 * this virtual method can be overrided in child class. such as ModifiedStat overrides this method
 * it is used to update @para total_value_ when @para base_value_ and @para buff_value_ change
 */
public class BaseStat
{
		protected int base_value_;
		protected int buff_value_;
		protected int exp2next_level_;
		protected float exp2next_level_modifier_;
		protected int total_value_;
		public BaseStat ()
		{
				base_value_ = GameDatabase.base_value_;
				buff_value_ = GameDatabase.buff_value_;
				total_value_ = GameDatabase.total_value_;
				exp2next_level_ = GameDatabase.exp2next_level_;
				exp2next_level_modifier_ = GameDatabase.exp2next_level_modifier_;
		}

		//public void LevelUp()
		//{
		//	exp2next_level_ = (int)(exp2next_level_ * exp2next_level_modifier_);
		//	++base_value_;
		//	Update();
		//}

		public virtual void Update ()
		{
				total_value_ = base_value_ + buff_value_;
		}

	#region getters and setters
		public int TotalValue {
				get { Update ();
						return total_value_; }
				set { total_value_ = value; }
		}

		public int BaseValue {
				get { return base_value_; }
				set { base_value_ = value; }
		}

		public int Buff_Value {
				get { return buff_value_; }
				set { buff_value_ = value; }
		}
		public int Exp2Nextlevel {
				get { return exp2next_level_; }
				set { exp2next_level_ = value; }
		}
		public float LevelModifier {
				get { return exp2next_level_modifier_; }
				set { exp2next_level_modifier_ = value; }
		}
	#endregion
}
