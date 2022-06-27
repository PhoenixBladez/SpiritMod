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
			Item.width = Item.height = 16;
			Item.rare = -11;
			Item.maxStack = 99;
		}

		public override bool OnPickup(Player player) => !player.HasItem(ModContent.ItemType<CrimsonDyeMaterial>());

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<StylistQuestSeafoam>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(Mod, "ItemName", "Quest Item") {
					OverrideColor = new Color(100, 222, 122)
				};
				tooltips.Add(line);
			}
			TooltipLine line1 = new TooltipLine(Mod, "FavoriteDesc", "'The disgusting organ makes strange pulsating motions'") {
				OverrideColor = new Color(255, 255, 255)
			};
			tooltips.Add(line1);
		}
	}
}
