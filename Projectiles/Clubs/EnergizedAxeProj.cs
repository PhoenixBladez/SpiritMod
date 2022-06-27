using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles.Magic;

namespace SpiritMod.Projectiles.Clubs
{
	class EnergizedAxeProj : ClubProj
	{
		public EnergizedAxeProj() : base(40, 23, 53, -1, 60, 6, 10, 4f, 19f) { }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Adze");
			Main.projFrames[Projectile.type] = 3;
		}

		public override void Smash(Vector2 position)
		{
			Player player = Main.player[Projectile.owner];
			for (int k = 0; k <= 100; k++)
				Dust.NewDustPerfect(Projectile.oldPosition + new Vector2(Projectile.width / 2, Projectile.height / 2), ModContent.DustType<Dusts.BoneDust>(), new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * Projectile.ai[0] / 9f);

			for (int k = 0; k <= 30; k++)
				Dust.NewDustPerfect(Projectile.oldPosition + new Vector2(Projectile.width / 2, Projectile.height / 2), 226, new Vector2(0, 1).RotatedByRandom(1) * Main.rand.NextFloat(-1, 1) * Projectile.ai[0] / 9f);
		}

		public override void SafeDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			int size = 60;
			if (Projectile.ai[0] >= ChargeTime)
			{

				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, size * 2, size, size), Color.White * 0.9f, TrueRotation, Origin, Projectile.scale, Effects, 1);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.ai[0] >= ChargeTime)
			{
				SoundEngine.PlaySound(SoundID.Item109);
				for (int i = 0; i < 20; i++)
				{
					int num = Dust.NewDust(target.position, target.width, target.height, DustID.Electric, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if (Main.dust[num].position != target.Center)
						Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 6f;
				}
				int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<GraniteSpike1>(), Projectile.damage, Projectile.knockBack / 2, Main.myPlayer, 0f, 0f);
				Main.projectile[proj].timeLeft = 2;
			}
		}
	}
}
