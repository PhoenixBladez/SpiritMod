using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.DiseasedSlime
{
	class NoxiousGas : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Noxious Field");

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 4;
			projectile.timeLeft = 90;
			projectile.height = 80;
			projectile.width = 80;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			float distance = Vector2.DistanceSquared(projectile.Center, player.Center);
			if (distance < 45 * 45)
			{
				player.AddBuff(ModContent.BuffType<FesteringWounds>(), 600);
				player.AddBuff(BuffID.Poisoned, 600);
			}

			if (Main.rand.NextBool(2))
			{
				Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, ModContent.DustType<Dusts.NoxiousDust>(), Main.rand.NextFloat(-1.5f, 1.5f), 0f, 100, new Color(), Main.rand.NextFloat(1.5f, 1.75f))];

			}
		}
    }
}
