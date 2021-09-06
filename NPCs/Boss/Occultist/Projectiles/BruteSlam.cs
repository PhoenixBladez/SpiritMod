using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using SpiritMod.Particles;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class BruteSlam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeleton Brute");
			Main.projFrames[projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			projectile.timeLeft = TOTALTIME;
			projectile.hostile = true;
			projectile.height = 112;
			projectile.width = 160;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.hide = true;
		}

		private const int FADEINTIME = 40;
		public const int TOTALTIME = 75;
		private const int DAMAGEFRAME = 5;

		private NPC Parent => Main.npc[(int)projectile.ai[0]];
		private ref float AiTimer => ref projectile.ai[1];

		public override void AI()
		{
			projectile.velocity.Y += 0.2f;
			projectile.alpha = Math.Max(projectile.alpha - 255 / FADEINTIME, 0);
			int temp = projectile.frame;
			projectile.UpdateFrame((int)(10 * 60f / TOTALTIME));
			projectile.spriteDirection = projectile.direction = -Parent.direction;
			if (projectile.frame != temp)
			{
				if (projectile.frame == DAMAGEFRAME)
				{
					BruteShockwave.MakeShockwave(projectile.Center, -projectile.direction, (int)(projectile.damage * 0.75f), 20);

					if (!Main.dedServ)
					{
						for (int i = 0; i < 4; i++)
							ParticleHandler.SpawnParticle(new ImpactLine(projectile.Bottom + new Vector2(Main.rand.NextFloat(45, 55) * -projectile.direction, 0),
								-Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(1, 2.5f), Color.White, new Vector2(0.33f, Main.rand.NextFloat(1f, 1.5f)), 15));
					}
				}

				projectile.netUpdate = true;
			}

			AiTimer++;
		}

		public override void Kill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				Main.PlaySound(SoundID.DD2_SkeletonHurt.WithPitchVariance(0.3f).WithVolume(0.5f), projectile.Center);

				for (int i = 1; i < 6; ++i)
					Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/SkeletonBrute/SkeletonBruteGore" + i), 1f);
			}
		}

		public override bool CanDamage() => projectile.frame == DAMAGEFRAME;

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI) => drawCacheProjsBehindNPCs.Add(index);

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(projectile.frame);

		public override void ReceiveExtraAI(BinaryReader reader) => projectile.frame = reader.ReadInt32();

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			projectile.QuickDrawGlow(spriteBatch, projectile.GetAlpha(new Color(252, 3, 148)) * Math.Max(1 - AiTimer / FADEINTIME, 0));
			return false;
		}
	}
}