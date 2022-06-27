﻿using Terraria;
using System;
using SpiritMod.Tiles.Block;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"

namespace SpiritMod.Items.Placeable.Tiles
{
	public class AsteroidBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Ore-rich space debris'");
        }

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Asteroid>();
		}
    }
}
