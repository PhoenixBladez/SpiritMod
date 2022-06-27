using SpiritMod.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	class BeetleArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beetle Arrow");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.owner == Main.myPlayer)
				Main.LocalPlayer.AddBuff(ModContent.BuffType<BeetleFortitude>(), 180);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			if (Projectile.owner == Main.myPlayer)
				Main.LocalPlayer.AddBuff(ModContent.BuffType<BeetleFortitude>(), 180);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BubbleBurst_Purple);
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

	}
}
