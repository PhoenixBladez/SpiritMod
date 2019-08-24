using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class PlagueSkullsHandle : ModProjectile
	{
		int SpeedMult = 36;
		int DamageSub;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Skulls");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 36;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.ignoreWater = true;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			float num = MathHelper.PiOver2;
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);

			float num26 = 30f;
			if (projectile.ai[0] > 90f)
				num26 = 15f;

			if (projectile.ai[0] > 120f)
				num26 = 5f;

			projectile.damage = (int)(player.inventory[player.selectedItem].damage * player.magicDamage);
			projectile.ai[0]++;
			projectile.ai[1]++;
			bool flag9 = false;


			if (projectile.ai[0] % SpeedMult == 0f)
			{
				Vector2 center3 = projectile.Center;
				Vector2 vector12 = Vector2.Normalize(projectile.velocity);
				if (float.IsNaN(vector12.X) || float.IsNaN(vector12.Y))
					vector12 = -Vector2.UnitY;

				int num29 = projectile.damage;
				Projectile.NewProjectile(center3.X, center3.Y,
					vector12.X * (Main.rand.Next(700, 900) / 100),
					vector12.Y * (Main.rand.Next(700, 900) / 100),
					mod.ProjectileType("PlagueSkullProj"),
					num29 - ((DamageSub / 3) * 2), projectile.knockBack, projectile.owner, 0, projectile.whoAmI);
				projectile.netUpdate = true;
				if (SpeedMult > 10)
				{
					SpeedMult--;
					DamageSub++;
					DamageSub++;
				}
			}

			if (projectile.ai[0] % num26 == 0f)
				flag9 = true;


			bool flag10 = false;
			if (projectile.ai[0] % num26 == 0f)
				flag10 = true;

			if (projectile.ai[1] >= 1f)
			{
				projectile.ai[1] = 0f;
				flag10 = true;
				if (Main.myPlayer == projectile.owner)
				{
					float scaleFactor5 = player.inventory[player.selectedItem].shootSpeed * projectile.scale;
					Vector2 value12 = vector;
					Vector2 value13 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - value12;
					if (player.gravDir == -1f)
						value13.Y = Main.screenHeight - Main.mouseY + Main.screenPosition.Y - value12.Y;

					Vector2 vector11 = Vector2.Normalize(value13);
					if (float.IsNaN(vector11.X) || float.IsNaN(vector11.Y))
						vector11 = -Vector2.UnitY;

					vector11 = Vector2.Normalize(Vector2.Lerp(vector11, Vector2.Normalize(projectile.velocity), 0.92f));
					vector11 *= scaleFactor5;
					if (vector11.X != projectile.velocity.X || vector11.Y != projectile.velocity.Y)
						projectile.netUpdate = true;

					projectile.velocity = vector11;
				}
			}

			int num28 = (projectile.ai[0] < 120f) ? 4 : 1;
			if (flag10 && Main.myPlayer == projectile.owner)
			{
				bool flag11 = !flag9 || player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
				bool flag12 = player.channel && flag11 && !player.noItems && !player.CCed;
				if (flag12)
				{
					if (projectile.ai[0] == 1f)
					{
						Vector2 center3 = projectile.Center;
						Vector2 vector12 = Vector2.Normalize(projectile.velocity);
						if (float.IsNaN(vector12.X) || float.IsNaN(vector12.Y))
							vector12 = -Vector2.UnitY;

						int num29 = projectile.damage;
						Projectile.NewProjectile(center3.X, center3.Y,
							vector12.X * (Main.rand.Next(700, 900) / 100),
							vector12.Y * (Main.rand.Next(700, 900) / 100),
							mod.ProjectileType("PlagueSkullProj"),
							num29 - DamageSub, projectile.knockBack, projectile.owner, 0, projectile.whoAmI);
						projectile.netUpdate = true;
					}
				}
				else
				{
					projectile.Kill();
				}
			}

			if (projectile.localAI[0] >= 120)
			{
				projectile.Kill();
				return false;
			}

			projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - projectile.Size / 2f;
			projectile.rotation = projectile.velocity.ToRotation() + num;
			projectile.spriteDirection = projectile.direction;
			projectile.timeLeft = 2;
			player.ChangeDir(projectile.direction);
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
			return false;
		}

	}
}
