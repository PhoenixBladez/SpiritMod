using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using SpiritMod.Tiles.Block;

namespace SpiritMod.Items.ByBiome.Forest.Consumeable
{
	internal class StarPowder : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starpowder");
			Tooltip.SetDefault("Can be thrown on grass to invigorate it");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 26;
			Item.rare = ItemRarityID.White;
			Item.maxStack = 99;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.reuseDelay = 10;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item43;
			Item.shoot = ModContent.ProjectileType<StarPowderProj>();
			Item.shootSpeed = 6f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(5)
				.AddIngredient(ItemID.Star, 1)
				.Register();
		}
	}

	internal class StarPowderProj : ModProjectile
	{
		public override string Texture => base.Texture[..^"Proj".Length];

		public override void SetDefaults()
		{
			Projectile.hide = true;
			Projectile.Size = new Vector2(40, 40);
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 80;
		}

		public override void AI()
		{
			Dust.NewDust(Projectile.position, 40, 40, DustID.BlueTorch, Projectile.velocity.X, Projectile.velocity.Y);

			ConvertTiles();
		}

		private void ConvertTiles()
		{
			Point pos = Projectile.position.ToTileCoordinates();
			Point end = Projectile.BottomRight.ToTileCoordinates();

			for (int i = pos.X; i < end.X; ++i)
			{
				for (int j = pos.Y; j < end.Y; ++j)
				{
					if (!WorldGen.InWorld(i, j))
						continue;

					Tile tile = Main.tile[i, j];

					if (tile.TileType == TileID.Grass)
						tile.TileType = (ushort)ModContent.TileType<Stargrass>();
				}
			}
		}
	}
}
