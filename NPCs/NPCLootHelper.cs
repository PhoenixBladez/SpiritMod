using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public static class NPCLootHelper
	{
		public static void Add(this NPCLoot loot, params IItemDropRule[] rules)
		{
			foreach (var item in rules)
				loot.Add(item);
		}

		public static void AddCommon(this NPCLoot loot, int itemID, int chanceDenominator = 1, int minStack = 1, int maxStack = 1)
		{
			if (maxStack < minStack)
				maxStack = minStack;
			loot.Add(ItemDropRule.Common(itemID, chanceDenominator, minStack, maxStack));
		}

		public static void AddFood(this NPCLoot loot, int itemID, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) => loot.Add(ItemDropRule.Food(itemID, chanceDenominator, minStack, maxStack));
		public static void AddOneFromOptions(this NPCLoot loot, int chanceDenominator, params int[] types) => loot.Add(ItemDropRule.OneFromOptions(chanceDenominator, types));
		public static void AddBossBag(this NPCLoot loot, int itemID) => loot.Add(ItemDropRule.BossBag(itemID));

		public static void AddCommon<T>(this NPCLoot loot, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) where T : ModItem => loot.AddCommon(ModContent.ItemType<T>(), chanceDenominator, minStack, maxStack);
		public static void AddFood<T>(this NPCLoot loot, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) where T : ModItem => loot.Add(ItemDropRule.Food(ModContent.ItemType<T>(), chanceDenominator, minStack, maxStack));
		public static void AddBossBag<T>(this NPCLoot loot) where T : ModItem => loot.Add(ItemDropRule.BossBag(ModContent.ItemType<T>()));
		public static void AddMasterModeCommonDrop<T>(this NPCLoot loot) where T : ModItem => loot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<T>()));

		public static LeadingConditionRule NightCondition(this NPCLoot loot) => new LeadingConditionRule(new DropRuleConditions.NotDay());

		//Uh on no params generics!!!
		public static void AddOneFromOptions<T1, T2>(this NPCLoot loot, int chance = 1) where T1 : ModItem where T2 : ModItem => loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>());
		public static void AddOneFromOptions<T1, T2, T3>(this NPCLoot loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem => loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>());
		public static void AddOneFromOptions<T1, T2, T3, T4>(this NPCLoot loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem where T4 : ModItem
			=> loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>(), ModContent.ItemType<T4>());
		public static void AddOneFromOptions<T1, T2, T3, T4, T5>(this NPCLoot loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem where T4 : ModItem where T5 : ModItem
			=> loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>(), ModContent.ItemType<T4>(), ModContent.ItemType<T5>());
		public static void AddOneFromOptions<T1, T2, T3, T4, T5, T6>(this NPCLoot loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem where T4 : ModItem where T5 : ModItem where T6 : ModItem
			=> loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>(), ModContent.ItemType<T4>(), ModContent.ItemType<T5>(), ModContent.ItemType<T6>());
		public static void AddOneFromOptions<T1, T2, T3, T4, T5, T6, T7>(this NPCLoot loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem where T4 : ModItem where T5 : ModItem where T6 : ModItem where T7 : ModItem
			=> loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>(), ModContent.ItemType<T4>(), ModContent.ItemType<T5>(), ModContent.ItemType<T6>(), ModContent.ItemType<T7>());
	}
}
