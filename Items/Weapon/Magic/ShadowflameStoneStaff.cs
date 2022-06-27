using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Magic;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ShadowflameStoneStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowbreak Wand");
			Tooltip.SetDefault("Shoots out erratic shadowflame wisps");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Weapon/Magic/ShadowflameStoneStaff_Glow");
		}


		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 46;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.damage = 12;
			Item.knockBack = 4;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.useTime = 12;
			Item.useAnimation = 24;
			Item.mana = 10;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = false;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<ShadowflameStoneBolt>();
			Item.shootSpeed = 10f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for (int I = 0; I < Main.rand.Next(1, 3); I++) {
				float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) {
					position += spawnPlace;
				}

				velocity = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
				int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, 0, 0.0f, 0.0f);
				for (float num2 = 0.0f; (double)num2 < 10; ++num2) {
					int dustIndex = Dust.NewDust(position, 2, 2, DustID.ShadowbeamStaff, 0f, 0f, 0, default, .8f);
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
				}
			}

			return false;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Weapon/Magic/ShadowflameStoneStaff_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}
