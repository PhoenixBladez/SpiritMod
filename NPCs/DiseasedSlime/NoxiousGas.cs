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
			projectile.height = 70;
			projectile.width = 70;
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

			if (Main.rand.NextBool(6))
			{
				for (int j = 0; j < 9; j++)
				{
					Dust dust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, ModContent.DustType<Dusts.NoxiousDust>(), 0f, 0f, 100, new Color(), Main.rand.NextFloat(2.5f, 4.5f))];
					dust.noGravity = true;
				}
			}
		}
    }
}
