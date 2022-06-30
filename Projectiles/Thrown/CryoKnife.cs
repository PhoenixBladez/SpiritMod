using SpiritMod.Buffs.DoT;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class CryoKnife : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Bomb");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 14;
			Projectile.aiStyle = 2;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 0;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item69, Projectile.Center);
			SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<CryoExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			for (int i = 0; i < 5; i++) {
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonSpirit);
				Main.dust[d].scale = .5f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(ModContent.BuffType<CryoCrush>(), 240);
		}

		public override bool PreAI() => true;
	}
}