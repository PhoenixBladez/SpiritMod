using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Tiles;

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
			item.rare = ItemRarityID.Orange;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
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
				for (int y = (int)Main.rockLayer; y < Main.tile.GetLength(1); ++y) 
				{
					if (Main.tile[x, y] == null) continue;
					if (Main.tile[x, y].type != mod.TileType("Fathomless_Chest")) continue;
					pos = new Vector2((x) * 16, y * 16);
					break;
				}
			}
			if (pos != prePos) {
				RunTeleport(player, new Vector2(pos.X, pos.Y));
			}
			else {
				CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 2, 2), Color.Cyan, "No Shrines Left!");
				player.QuickSpawnItem(ModContent.ItemType<Black_Stone_Item>(), 15);
				Main.PlaySound(SoundID.Item110, player.Center);
				return;
			}
		}
		private void RunTeleport(Player player, Vector2 pos)
		{
			player.Teleport(pos, 2, 0);
			player.velocity = Vector2.Zero;
			Main.PlaySound(SoundID.Item6, player.Center);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Black_Stone_Item", 18);	
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}