using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Dusts;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class FlayedShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flayed Bolt");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.width = 14;
			Projectile.height = 30;
		}
		public override void AI()
		{
			int num = 5;
			for (int k = 0; k < 5; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Blood, 0.0f, 0.0f, 0, new Color(), 1.6f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<BloodInfusion>(), 151, true);
			if (target.life < damage - (target.defense / 2)) {
				Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<FlayedExplosion>(), 25, 0, Main.myPlayer);
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<NightmareDust>());
			}
			SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y);
		}

	}
}