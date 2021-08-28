using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.BowsMisc.Carrion
{
	public class Carrion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carrion");
			Tooltip.SetDefault("Converts wooden arrows into Carrion Crows\nCarrion Crows grow stronger after hitting enemies");
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			item.damage = 35;
			item.noMelee = true;
			item.ranged = true;
			item.width = 50;
			item.height = 30;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shootSpeed = 12f;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) 
				type = ModContent.ProjectileType<CarrionCrowArrow>();

			float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
			Vector2 spawnPlace = (type == ModContent.ProjectileType<CarrionCrowArrow>()) ? Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 45f : Vector2.Zero;
			if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0))
				position += spawnPlace;

			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - position) * item.shootSpeed;
			Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, 0, 0.0f, 0.0f);
			for (float num2 = 0.0f; (double)num2 < 10; ++num2)
			{
				int dustIndex = Dust.NewDust(position, 2, 2, DustID.Wraith, 0f, 0f, 0, default, 1f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
			}
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-14, 0);
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellwingBow, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.RottenChunk, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ItemID.HellwingBow, 1);
			recipe1.AddIngredient(ItemID.SoulofNight, 12);
			recipe1.AddIngredient(ItemID.Vertebrae, 10);
			recipe1.AddTile(TileID.MythrilAnvil);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();
		}
	}
}
