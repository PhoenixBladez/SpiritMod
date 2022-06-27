using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Reach
{
	public class ThornKnife : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn Knife");
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;

			Projectile.damage = 3;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = true;
			Projectile.alpha = 0;
		}
		bool summoned = false;
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (!summoned) {
				for (int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<FloranDust2>(), 0f, 0f, 160, new Color(), 1f);
					// Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
				summoned = true;
			}
			if (Main.rand.NextBool(3)) {
				Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<FloranDust2>());
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center, 1);
		}
	}
}
