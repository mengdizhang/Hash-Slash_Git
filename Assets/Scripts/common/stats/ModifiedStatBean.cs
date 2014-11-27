/**
 * ModifiedSta.cs
 * 
 * by Jackie Zhang on 23/10/2014
 * 
 * @class ModifyingAttribute
 *  This class is used as container holding a Attribute and a ratio 
 *  that will be added to @class ModifiedStat
 * 
 * @class ModifiedStat
 * This class is used to represent a stat that is effected by some @class ModifyingAttribute,
 * 
 * @para modifiers_
 * A list of attri that will modify this stat
 * 
 * @para adding_value_
 * Amounts incresed from @para modifiers_
 * 
 * @method AddModifier(ModifyingAttribute ma)
 * Used to add a related @class ModifyingAttribute instance to @para modifiers_
 * 
 * @method Update()
 * Overrides @method Update() in @class BaseStat
 * It will recalculate the @para adding_value_  and also call base.update()
 * Then it will update @para total_value_.
 * 
 * @method Modifiers
 * Getter of @para modifiers_
 */
using System.Collections.Generic;
public struct ModifyingAttribute
{
		public CharacterAttribute attri_; // might
		public float ratio_;

		public ModifyingAttribute (CharacterAttribute attri, float ratio)
		{
				attri_ = attri;
				ratio_ = ratio;
		}
}

public class ModifiedStatBean : BaseStat
{
		private List<ModifyingAttribute> modifiers_;
		private int adding_value_;

		public ModifiedStatBean ()
		{
				modifiers_ = new List<ModifyingAttribute> ();
				adding_value_ = 0;
		}

		public ModifyingAttribute[] Modifiers {
				get {
						return modifiers_.ToArray ();
				}
		}

		public void AddModifier (ModifyingAttribute ma)
		{
				modifiers_.Add (ma);
		}

		public override void UpdateStat ()
		{
				//update total value
				base.UpdateStat ();

				adding_value_ = 0; //reset to 0 before recalculation
				if (modifiers_.Count > 0) {
						foreach (ModifyingAttribute mattr in modifiers_) {
								adding_value_ += (int)(mattr.attri_.TotalValue * mattr.ratio_);
						}
				}
				//update total value
				total_value_ += adding_value_;
		}
}

