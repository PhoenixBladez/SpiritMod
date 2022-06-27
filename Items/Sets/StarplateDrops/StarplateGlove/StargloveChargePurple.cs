using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops.StarplateGlove
{
	public class StargloveChargePurple : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfall");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
			Projectile.scale = 1f;
			Projectile.tileCollide = true;
			Projectile.hide = false;
			Projectile.extraUpdates = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 20;
			Projectile.ignoreWater = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			DustHelper.DrawStar(Projectile.Center, 111, 5, 1.5f, 1, 1, 1, 0.5f, true);
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.velocity.X < 0f)
				Projectile.spriteDirection = -1;
			else
				Projectile.spriteDirection = 1;
			++Projectile.ai[1];
			bool flag2 = Vector2.Distance(Projectile.Center, player.Center) > 0f && Projectile.Center.Y == player.Center.Y;
			if (Projectile.ai[1] >= 30f && flag2)
			{
				Projectile.ai[1] = 0.0f;
			}

			float num = 3f;
			for (int index1 = 0; index1 < num; ++index1)
			{
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Clentaminator_Cyan, 0.0f, 0.0f, 0, Color.Purple, 1.8f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * 5f * index1;
				Main.dust[index2].velocity *= 0.0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].alpha = 125;
				Main.dust[index2].scale = 0.8f;
			}
		}
	}
}