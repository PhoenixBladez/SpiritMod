using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.Fathomless_Chest
{
	public class Mystical_Dice : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystical Dice");
			Tooltip.SetDefault("Teleports you to a Fathomless Shrine");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 999;
			item.rare = 8;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item6;
			item.consumable = true;
		}
		public override bool UseItem(Player player)
		{
			FathomlessChestPos(player);
			return true;
		}
		private void FathomlessChestPos(Player player)
		{
			Vector2 prePos = player.position;
			Vector2 pos = prePos;
			for (int x = 0; x < Main.tile.GetLength(0); ++x)
			{
				for (int y = 0; y < Main.tile.GetLength(1); ++y) 
				{
					if (Main.tile[x, y] == null) continue;
					if (Main.tile[x, y].type != mod.TileType("Fathomless_Chest")) continue;
					pos = new Vector2((x) * 16, y * 16);
					break;
				}
			}
			if (pos != prePos)
			{
				RunTeleport(player, new Vector2(pos.X, pos.Y));
			}
			else return;
		}
		private void RunTeleport(Player player, Vector2 pos)
		{
			player.Teleport(pos, 2, 0);
			player.velocity = Vector2.Zero;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Black_Stone_Item", 15);	
			recipe.AddIngredient(182, 5);
			recipe.AddIngredient(178, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}