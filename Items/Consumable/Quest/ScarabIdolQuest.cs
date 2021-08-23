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
			item.width = item.height = 16;
			item.rare = ItemRarityID.Green;
			item.maxStack = 99;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<IdleIdol>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item");
				line.overrideColor = new Color(100, 222, 122);
				tooltips.Add(line);
			}
		}
	}
}
