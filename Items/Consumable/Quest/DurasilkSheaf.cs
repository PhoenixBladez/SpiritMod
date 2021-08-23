using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.Items.Consumable.Quest
{
	public class DurasilkSheaf : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Durasilk Sheaf");

		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
		}

		public override bool OnPickup(Player player) => !player.HasItem(ModContent.ItemType<DurasilkSheaf>());

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<FirstAdventure>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item") {
					overrideColor = new Color(100, 222, 122)
				};
				tooltips.Add(line);
			}
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "'The fabric is soft, but quite strong'") {
				overrideColor = new Color(255, 255, 255)
			};
			tooltips.Add(line1);
		}
	}
}
