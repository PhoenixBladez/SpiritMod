using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritPaintingTile = SpiritMod.Tiles.Furniture.SpiritPainting;
namespace SpiritMod.Items.Placeable.Furniture
{
	public class SpiritPainting : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Milestone");
		}

		public override void SetDefaults()
		{
			Item.width = 94;
			Item.height = 62;
			Item.value = Item.value = Terraria.Item.buyPrice(0, 10, 1000, 10);
			Item.rare = ItemRarityID.LightPurple;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<SpiritPaintingTile>();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(Mod, "ItemName", "'Quite a milestone indeed!\nThings can only look up from here'");
			line.OverrideColor = new Color(50, 80, 200);
			tooltips.Add(line);
			foreach (TooltipLine line2 in tooltips) {
				if (line2.Mod == "Terraria" && line2.Name == "ItemName") {
					line2.OverrideColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				}
			}
		}
	}
}