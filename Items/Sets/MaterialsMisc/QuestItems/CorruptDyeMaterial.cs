using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.Items.Sets.MaterialsMisc.QuestItems
{
	public class CorruptDyeMaterial : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Violet Crystal");

		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = -11;
			item.maxStack = 99;
		}

		public override bool OnPickup(Player player) => !player.HasItem(ModContent.ItemType<CorruptDyeMaterial>());

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<StylistQuestSeafoam>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item") {
					overrideColor = new Color(100, 222, 122)
				};
				tooltips.Add(line);
			}
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "'The crystal is both beautiful and disgsting to look at'") {
				overrideColor = new Color(255, 255, 255)
			};
			tooltips.Add(line1);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}
