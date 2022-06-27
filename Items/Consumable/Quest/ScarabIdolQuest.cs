using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.ID;

namespace SpiritMod.Items.Consumable.Quest
{
	public class ScarabIdolQuest : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Decrepit Idol");

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 99;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<IdleIdol>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(Mod, "ItemName", "Quest Item");
				line.OverrideColor = new Color(100, 222, 122);
				tooltips.Add(line);
			}
		}
	}
}
