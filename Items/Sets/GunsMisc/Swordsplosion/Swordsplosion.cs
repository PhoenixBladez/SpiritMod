using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.Swordsplosion
{
	public class Swordsplosion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swordsplosion");
			Tooltip.SetDefault("Shoots out a barrage of swords\nProjectiles fired count both as melee and ranged projectiles");
		}

		public override void SetDefaults()
		{
			Item.damage = 76;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 60;
			Item.height = 26;
			Item.useTime = 19;
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.useTurn = false;
			Item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item36;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SwordBarrage>();
			Item.shootSpeed = 10f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for (int i = 0; i < 3; i++) {
				float spread = 35f * 0.0174f;//45 degrees converted to radians
				float baseSpeed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
				double baseAngle = Math.Atan2(velocity.X, velocity.Y);
				double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
				velocity.X = baseSpeed * (float)Math.Sin(randomAngle);
				velocity.Y = baseSpeed * (float)Math.Cos(randomAngle);
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SwordBarrage>(), Item.damage, knockback, Item.playerIndexTheItemIsReservedFor, 0, 0);
			}
			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}