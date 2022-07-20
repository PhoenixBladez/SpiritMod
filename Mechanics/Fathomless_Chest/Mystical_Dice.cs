using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
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
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Orange;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		public override bool? UseItem(Player player)
		{
			FathomlessChestPos(player);
			return true;
		}

		private void FathomlessChestPos(Player player)
		{
			Vector2 prePos = player.position;
			Vector2 pos = prePos;

			for (int x = 0; x < Main.maxTilesX; ++x)
			{
				for (int y = (int)Main.rockLayer; y < Main.maxTilesY; ++y)
				{
					if (Main.tile[x, y] == null) 
						continue;

					if (Main.tile[x, y].TileType != Mod.Find<ModTile>("Fathomless_Chest").Type) 
						continue;

					pos = new Vector2(x * 16, y * 16);
					break;
				}
			}

			if (pos != prePos)
				RunTeleport(player, new Vector2(pos.X, pos.Y));
			else
			{
				CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 2, 2), Color.Cyan, "No Shrines Left!");
				player.QuickSpawnItem(player.GetSource_ItemUse(Item), ModContent.ItemType<Black_Stone_Item>(), 15);
				SoundEngine.PlaySound(SoundID.Item110, player.Center);
				return;
			}
		}

		private void RunTeleport(Player player, Vector2 pos)
		{
			player.Teleport(pos, 2, 0);
			player.velocity = Vector2.Zero;
			SoundEngine.PlaySound(SoundID.Item6, player.Center);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "Black_Stone_Item", 18);
			recipe.Register();
		}
	}
}