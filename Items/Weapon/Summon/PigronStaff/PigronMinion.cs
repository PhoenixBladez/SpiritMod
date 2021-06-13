using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.PigronStaff
{
	[AutoloadMinionBuff("Pigron Minion", "Bacon!")]
	public class PigronMinion : BaseMinion
	{
		public PigronMinion() : base(700, 1800, new Vector2(30, 30)) { }
		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Pigron");
			Main.projFrames[projectile.type] = 14;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override bool DoAutoFrameUpdate(ref int framespersecond)
		{
			framespersecond = 14;
			return true;
		}

		public override bool PreAI()
		{
			projectile.direction = projectile.spriteDirection = Math.Sign(-projectile.velocity.X);
			projectile.rotation = projectile.velocity.ToRotation() + (projectile.direction > 0 ? MathHelper.Pi : 0);
			if (Main.rand.Next(600) == 0 && Main.netMode != NetmodeID.Server)
				Main.PlaySound(SoundID.Zombie, (int)projectile.Center.X, (int)projectile.Center.Y, Main.rand.Next(39, 41), 0.33f, 0.5f);

			return true;
		}

		public override void IdleMovement(Player player)
		{
			if(projectile.Distance(player.Center) > 70)
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(player.Center) * MathHelper.Clamp(projectile.Distance(player.Center) / 50, 8, 14), 0.04f / (IndexOfType/3f + 1));

			if (projectile.Distance(player.Center) > 1800)
			{
				projectile.Center = player.Center;
				projectile.netUpdate = true;
			}

			projectile.ai[0] = 0;
			projectile.ai[1] = 0;
			projectile.alpha = Math.Max(projectile.alpha - 8, 0);
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			projectile.ai[0] = 1;
			if(Main.rand.Next(9) == 0 && projectile.velocity.Length() > 7)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 255, projectile.velocity.X / 3, projectile.velocity.Y / 3, 100, default, Main.rand.NextFloat(0.7f, 1.2f));
				dust.fadeIn = 0.8f;
				dust.noGravity = true;
			}

			switch (projectile.ai[1]) 
			{
				case 0:
					projectile.alpha += Math.Max(16 - IndexOfType/2, 12);
					projectile.velocity *= 0.97f;
					if (projectile.alpha >= 255)
					{
						projectile.ai[1]++;
						projectile.alpha = 255;
						projectile.Center = target.Center + target.DirectionTo(player.Center).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(350, 400);
						projectile.velocity = projectile.DirectionTo(target.Center) * Main.rand.NextFloat(11, 16);
						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(SoundID.DD2_WyvernDiveDown.WithPitchVariance(0.3f).WithVolume(0.5f), projectile.Center);
						projectile.netUpdate = true;
					}
					break;
				case 1:
					projectile.velocity *= 1.03f;
					projectile.alpha -= Math.Max(16 - IndexOfType/2, 12);
					if (projectile.alpha <= 0)
					{
						projectile.alpha = 0;
						projectile.netUpdate = true;
						projectile.ai[1] = 0;
					}
					break;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if(projectile.ai[0] == 1)
				projectile.QuickDrawTrail(spriteBatch);
			projectile.QuickDraw(spriteBatch);
			return false;
		}
	}
}