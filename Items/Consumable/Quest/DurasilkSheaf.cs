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
			Item.autoReuse = false;
			Item.useTurn = true;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.value = Item.buyPrice(0, 0, 3, 0);
			Item.createTile = ModContent.TileType<Tiles.Furniture.DurasilkSheafTile>();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!QuestManager.GetQuest<FirstAdventure>().IsCompleted)
			{
				TooltipLine line = new TooltipLine(Mod, "ItemName", "Quest Item") {
					OverrideColor = new Color(100, 222, 122)
				};
				tooltips.Add(line);
			}
			TooltipLine line1 = new TooltipLine(Mod, "FavoriteDesc", "'The fabric is soft, but quite strong'") {
				OverrideColor = new Color(255, 255, 255)
			};
			tooltips.Add(line1);
		}
	}
}
