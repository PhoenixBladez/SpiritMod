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

		//Non-generic extension methods
		//NPCLoot
		public static void AddFood(this NPCLoot loot, int itemID, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) => loot.Add(ItemDropRule.Food(itemID, chanceDenominator, minStack, maxStack));
		public static void AddOneFromOptions(this NPCLoot loot, int chanceDenominator, params int[] types) => loot.Add(ItemDropRule.OneFromOptions(chanceDenominator, types));
		public static void AddBossBag(this NPCLoot loot, int itemID) => loot.Add(ItemDropRule.BossBag(itemID));

		//LeadingConditionRule
		public static void AddFood(this LeadingConditionRule loot, int itemID, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) => loot.OnSuccess(ItemDropRule.Food(itemID, chanceDenominator, minStack, maxStack));
		public static void AddOneFromOptions(this LeadingConditionRule loot, int chanceDenominator, params int[] types) => loot.OnSuccess(ItemDropRule.OneFromOptions(chanceDenominator, types));
		public static void AddBossBag(this LeadingConditionRule loot, int itemID) => loot.OnSuccess(ItemDropRule.BossBag(itemID));

		//Generic extension methods
		//NPCLoot
		public static void AddCommon<T>(this NPCLoot loot, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) where T : ModItem => loot.AddCommon(ModContent.ItemType<T>(), chanceDenominator, minStack, maxStack);
		public static void AddFood<T>(this NPCLoot loot, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) where T : ModItem => loot.Add(ItemDropRule.Food(ModContent.ItemType<T>(), chanceDenominator, minStack, maxStack));
		public static void AddBossBag<T>(this NPCLoot loot) where T : ModItem => loot.Add(ItemDropRule.BossBag(ModContent.ItemType<T>()));
		public static void AddMasterModeCommonDrop<T>(this NPCLoot loot) where T : ModItem => loot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<T>()));
		public static void AddMasterModeDropOnAllPlayers<T>(this NPCLoot loot) where T : ModItem => loot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<T>()));

		public static void AddMasterModeRelicAndPet<TRelic, TPet>(this NPCLoot loot)
		{
			loot.AddMasterModeDropOnAllPlayers<TRelic>();
			loot.AddBossBag<TPet>();
		}
		
		//LeadingConditionRule
		public static void AddCommon<T>(this LeadingConditionRule loot, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) where T : ModItem => loot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<T>(), chanceDenominator, minStack, maxStack));
		public static void AddFood<T>(this LeadingConditionRule loot, int chanceDenominator = 1, int minStack = 1, int maxStack = 1) where T : ModItem => loot.OnSuccess(ItemDropRule.Food(ModContent.ItemType<T>(), chanceDenominator, minStack, maxStack));
		public static void AddBossBag<T>(this LeadingConditionRule loot) where T : ModItem => loot.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<T>()));
		public static void AddMasterModeCommonDrop<T>(this LeadingConditionRule loot) where T : ModItem => loot.OnSuccess(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<T>()));
		public static void AddMasterModeDropOnAllPlayers<T>(this LeadingConditionRule loot) where T : ModItem => loot.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<T>()));

		//Shortcut for getting a leading condition rule
		public static LeadingConditionRule NightCondition(this NPCLoot loot) => new LeadingConditionRule(new DropRuleConditions.NotDay());

		//Uh on no params generics!!! - OneFromOptions generic impl.
		//NPCLoot
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

		//LeadingConditionRule
		public static void AddOneFromOptions<T1, T2>(this LeadingConditionRule loot, int chance = 1) where T1 : ModItem where T2 : ModItem => loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>());
		public static void AddOneFromOptions<T1, T2, T3>(this LeadingConditionRule loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem => loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>());
		public static void AddOneFromOptions<T1, T2, T3, T4>(this LeadingConditionRule loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem where T4 : ModItem
			=> loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>(), ModContent.ItemType<T4>());
		public static void AddOneFromOptions<T1, T2, T3, T4, T5>(this LeadingConditionRule loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem where T4 : ModItem where T5 : ModItem
			=> loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>(), ModContent.ItemType<T4>(), ModContent.ItemType<T5>());
		public static void AddOneFromOptions<T1, T2, T3, T4, T5, T6>(this LeadingConditionRule loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem where T4 : ModItem where T5 : ModItem where T6 : ModItem
			=> loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>(), ModContent.ItemType<T4>(), ModContent.ItemType<T5>(), ModContent.ItemType<T6>());
		public static void AddOneFromOptions<T1, T2, T3, T4, T5, T6, T7>(this LeadingConditionRule loot, int chance = 1) where T1 : ModItem where T2 : ModItem where T3 : ModItem where T4 : ModItem where T5 : ModItem where T6 : ModItem where T7 : ModItem
			=> loot.AddOneFromOptions(chance, ModContent.ItemType<T1>(), ModContent.ItemType<T2>(), ModContent.ItemType<T3>(), ModContent.ItemType<T4>(), ModContent.ItemType<T5>(), ModContent.ItemType<T6>(), ModContent.ItemType<T7>());
	}
}
