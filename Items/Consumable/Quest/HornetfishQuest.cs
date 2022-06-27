using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Quests;
using SpiritMod.Mechanics.QuestSystem;

namespace SpiritMod.Items.Consumable.Quest
{
	public class HornetfishQuest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hornetfish");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = -11;
			Item.maxStack = 99;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<ItsNoSalmon>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(Mod, "ItemName", "Quest Item");
				line.OverrideColor = new Color(100, 222, 122);
				tooltips.Add(line);
			}
			TooltipLine line1 = new TooltipLine(Mod, "FavoriteDesc", "'It buzzes and blubs at the same time'");
			line1.OverrideColor = new Color(255, 255, 255);
			tooltips.Add(line1);
		}
	}
}
