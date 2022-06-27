using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace SpiritMod
{
	public class LootConditions
	{
		public class NotDay : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => !Main.dayTime;
			public bool CanShowItemDropInUI() => true;
			public string GetConditionDescription() => null;
		}

		public class Day : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => Main.dayTime;
			public bool CanShowItemDropInUI() => true;
			public string GetConditionDescription() => null;
		}
	}
}
