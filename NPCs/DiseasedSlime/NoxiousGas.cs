using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.DiseasedSlime
{
	class NoxiousGas : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Noxious Field");

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 90;
			Projectile.height = 80;
			Projectile.width = 80;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			float distance = Vector2.DistanceSquared(Projectile.Center, player.Center);
			if (distance < 45 * 45)
			{
				player.AddBuff(ModContent.BuffType<FesteringWounds>(), 600);
				player.AddBuff(BuffID.Poisoned, 600);
			}

			if (Main.rand.NextBool(2))
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.NoxiousDust>(), Main.rand.NextFloat(-1.5f, 1.5f), 0f, 100, new Color(), Main.rand.NextFloat(1.5f, 1.75f));
		}
    }
}
