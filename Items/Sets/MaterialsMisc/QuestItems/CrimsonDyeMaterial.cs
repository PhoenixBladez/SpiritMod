using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.Items.Sets.MaterialsMisc.QuestItems
{
	public class CrimsonDyeMaterial : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bloody Tumor");

		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = -11;
			item.maxStack = 99;
		}

		public override bool OnPickup(Player player) => !player.HasItem(ModContent.ItemType<CrimsonDyeMaterial>());

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<StylistQuestSeafoam>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(mod, "ItemName", "Quest Item") {
					overrideColor = new Color(100, 222, 122)
				};
				tooltips.Add(line);
			}
			TooltipLine line1 = new TooltipLine(mod, "FavoriteDesc", "'The disgusting organ makes strange pulsating motions'") {
				overrideColor = new Color(255, 255, 255)
			};
			tooltips.Add(line1);
		}
	}
}
