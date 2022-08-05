using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.BowsMisc.Carrion
{
	public class Carrion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carrion");
			Tooltip.SetDefault("Converts wooden arrows into Carrion Crows\nCarrion Crows grow stronger after hitting enemies");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			Item.damage = 35;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 50;
			Item.height = 30;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 3;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, rotation, scale);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (type == ProjectileID.WoodenArrowFriendly) 
				type = ModContent.ProjectileType<CarrionCrowArrow>();

			float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
			Vector2 spawnPlace = (type == ModContent.ProjectileType<CarrionCrowArrow>()) ? Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 45f : Vector2.Zero;
			if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0))
				position += spawnPlace;

			velocity = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, 0, 0.0f, 0.0f);
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
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.HellwingBow, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.RottenChunk, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ItemID.HellwingBow, 1);
			recipe1.AddIngredient(ItemID.SoulofNight, 12);
			recipe1.AddIngredient(ItemID.Vertebrae, 10);
			recipe1.AddTile(TileID.MythrilAnvil);
			recipe1.Register();
		}
	}
}
