using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class ArcEnCielHandle : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arc-en-Ciel");
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			float num = MathHelper.PiOver2;
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);

			float num26 = 30f;
			if (Projectile.ai[0] > 90f) {
				num26 = 15f;
			}
			if (Projectile.ai[0] > 120f) {
				num26 = 5f;
			}
			Projectile.damage = (int)(player.GetDamage(DamageClass.Magic).ApplyTo(player.inventory[player.selectedItem].damage));
			Projectile.ai[0]++;
			Projectile.ai[1]++;
			bool flag9 = false;
			if (Projectile.ai[0] % num26 == 0f)
				flag9 = true;

			int num27 = 10;
			bool flag10 = false;
			if (Projectile.ai[0] % num26 == 0f)
				flag10 = true;

			if (Projectile.ai[1] >= 1f) {
				Projectile.ai[1] = 0f;
				flag10 = true;
				if (Main.myPlayer == Projectile.owner) {
					float scaleFactor5 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
					Vector2 value12 = vector;
					Vector2 value13 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - value12;
					if (player.gravDir == -1f)
						value13.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - value12.Y;

					Vector2 vector11 = Vector2.Normalize(value13);
					if (vector11.HasNaNs())
						vector11 = -Vector2.UnitY;

					vector11 = Vector2.Normalize(Vector2.Lerp(vector11, Vector2.Normalize(Projectile.velocity), 0.92f));
					vector11 *= scaleFactor5;
					if (vector11.X != Projectile.velocity.X || vector11.Y != Projectile.velocity.Y)
						Projectile.netUpdate = true;

					Projectile.velocity = vector11;
				}
			}

			int num28 = (Projectile.ai[0] < 120f) ? 4 : 1;
			if (Projectile.soundDelay <= 0) {
				Projectile.soundDelay = num27;
				Projectile.soundDelay *= 2;
				if (Projectile.ai[0] != 1f)
					SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
			}

			if (flag10 && Main.myPlayer == Projectile.owner) {
				bool flag11 = !flag9 || player.CheckMana(player.inventory[player.selectedItem].mana, true, false);
				bool flag12 = player.channel && flag11 && !player.noItems && !player.CCed;
				if (flag12) {
					if (Projectile.ai[0] == 1f) {
						Vector2 center3 = Projectile.Center;
						Vector2 vector12 = Vector2.Normalize(Projectile.velocity);
						if (vector12.HasNaNs())
							vector12 = -Vector2.UnitY;

						int num29 = Projectile.damage;
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), center3.X, center3.Y, vector12.X, vector12.Y, ModContent.ProjectileType<ArcEnCielProj>(),
							   num29, Projectile.knockBack, Projectile.owner, 0, Projectile.whoAmI);
						Projectile.netUpdate = true;
					}
				}
				else {
					Projectile.Kill();
				}
			}

			if (Projectile.localAI[0] >= 120) {
				Projectile.Kill();
				return false;
			}

			Projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - Projectile.Size / 2f;
			Projectile.rotation = Projectile.velocity.ToRotation() + num;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.timeLeft = 2;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction));
			return false;
		}

	}
}
