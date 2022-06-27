using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Projectiles.Magic;

namespace SpiritMod.Projectiles.Flail
{
	public class GraniteMaceProj : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Unstable Colonnade");

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override bool PreAI()
		{
			Vector2 position = Projectile.Center + Vector2.Normalize(Projectile.velocity) * 10;

			Dust newDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 0, default, .31f)];
			newDust.position = position;
			newDust.velocity = Projectile.velocity.RotatedBy(Math.PI / 2, default) * 0.33F + Projectile.velocity / 4;
			newDust.position += Projectile.velocity.RotatedBy(Math.PI / 2, default);
			newDust.fadeIn = 0.5f;
			newDust.noGravity = true;
			newDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 0, default, .31f)];
			newDust.position = position;
			newDust.velocity = Projectile.velocity.RotatedBy(-Math.PI / 2, default) * 0.33F + Projectile.velocity / 4;
			newDust.position += Projectile.velocity.RotatedBy(-Math.PI / 2, default);
			newDust.fadeIn = 0.5F;
			newDust.noGravity = true;
			ProjectileExtras.FlailAI(Projectile.whoAmI);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => ProjectileExtras.FlailTileCollide(Projectile.whoAmI, oldVelocity);

		public override bool PreDraw(ref Color lightColor)
		{
		 	ProjectileExtras.DrawChain(Projectile.whoAmI, Main.player[Projectile.owner].MountedCenter, "SpiritMod/Projectiles/Flail/GraniteMace_Chain");
			ProjectileExtras.DrawAroundOrigin(Projectile.whoAmI, lightColor);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.life <= 0)
			{
				if (Projectile.friendly && !Projectile.hostile)
					ProjectileExtras.Explode(Projectile.whoAmI, 30, 30, () => { });

				SoundEngine.PlaySound(SoundID.Item109);

				for (int i = 0; i < 20; i++)
				{
					int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if (Main.dust[num].position != Projectile.Center)
						Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}

				int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<GraniteSpike1>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
				Main.projectile[proj].timeLeft = 2;
			}
		}
	}
}
