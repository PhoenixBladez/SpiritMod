using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ValkyrieSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Spirit Spear");
			Tooltip.SetDefault("Deals both magic and melee damage");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 11;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<ValkyrieSpearFriendly>();
			Item.shootSpeed = 11.5f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for (int I = 0; I < 2; I++) {
				float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) {
					position += spawnPlace;
				}

				velocity = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
				int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, 0, 0.0f, 0.0f);
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);

				for (float num2 = 0.0f; (double)num2 < 10; ++num2) {
					int dustIndex = Dust.NewDust(position, 2, 2, DustID.PortalBolt, 0f, 0f, 0, default, 1f);
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
				}
			}

			return false;
		}
	}
}
