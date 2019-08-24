using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Gun
{
	public class Thresher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Thresher");
			Tooltip.SetDefault("'Fires a blast of bullets and cursed flame'");
		}


		public override void SetDefaults()
		{
			item.damage = 29;
			item.ranged = true;
			item.width = 65;
			item.height = 21;
			item.useTime = 38;
			item.useAnimation = 38;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 7;

			item.useTurn = false;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item36;
			item.autoReuse = false;
			item.shoot = 10;
			item.shootSpeed = 8f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int projectileFired = Projectile.NewProjectile(position.X, position.Y, speedX / 2, speedY / 2, ProjectileID.EyeFire, item.damage, item.knockBack, player.whoAmI);
			Main.projectile[projectileFired].friendly = true;
			Main.projectile[projectileFired].friendly = true;
			Main.projectile[projectileFired].GetGlobalProjectile<SpiritGlobalProjectile>(mod).shotFromSpazLung = true;
			Main.projectile[projectileFired].hostile = false;
			Main.projectile[projectileFired].timeLeft = 20;
			Main.projectile[projectileFired].netUpdate = true;
			Vector2 origVect = new Vector2(speedX, speedY);
			for (int X = 0; X <= 3; X++)
			{
				Vector2 vel;
				if (Main.rand.Next(2) == 1)
					vel = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				else
					vel = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

	}
}
