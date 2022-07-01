using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public static class NPCLootHelper
	{
		public static void AddCommon(this NPCLoot loot, int itemID, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) => loot.Add(ItemDropRule.Common(itemID, chanceDenominator, minStack, maxStack));
		public static void AddFood(this NPCLoot loot, int itemID, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) => loot.Add(ItemDropRule.Food(itemID, chanceDenominator, minStack, maxStack));
		public static void AddOneFromOptions(this NPCLoot loot, int chanceDenominator, params int[] types) => loot.Add(ItemDropRule.OneFromOptions(chanceDenominator, types));

		public static void AddCommon<T>(this NPCLoot loot, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) where T : ModItem => loot.Add(ItemDropRule.Common(ModContent.ItemType<T>(), chanceDenominator, minStack, maxStack));
		public static void AddFood<T>(this NPCLoot loot, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) where T : ModItem => loot.Add(ItemDropRule.Food(ModContent.ItemType<T>(), chanceDenominator, minStack, maxStack));

		public static LeadingConditionRule NightCondition(this NPCLoot loot) => new LeadingConditionRule(new DropRuleConditions.NotDay());
	}
}
