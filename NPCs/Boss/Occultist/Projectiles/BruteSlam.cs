using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using SpiritMod.Particles;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class BruteSlam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brutish Slam");
			Main.projFrames[Projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			Projectile.timeLeft = TOTALTIME;
			Projectile.hostile = true;
			Projectile.height = 112;
			Projectile.width = 160;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.hide = true;
		}


		private const int FADEINTIME = 40;
		public const int TOTALTIME = 75;
		private const int DAMAGEFRAME = 5;

		private NPC Parent => Main.npc[(int)Projectile.ai[0]];
		private ref float AiTimer => ref Projectile.ai[1];

		public override void AI()
		{
			Projectile.velocity.Y += 0.2f;
			Projectile.alpha = Math.Max(Projectile.alpha - 255 / FADEINTIME, 0);
			int temp = Projectile.frame;
			Projectile.UpdateFrame((int)(10 * 60f / TOTALTIME));
			Projectile.spriteDirection = Projectile.direction = -Parent.direction;
			if (Projectile.frame != temp)
			{
				if (Projectile.frame == DAMAGEFRAME)
				{
					BruteShockwave.MakeShockwave(Projectile, Projectile.Center, -Projectile.direction, (int)(Projectile.damage * 0.75f), 20);

					if (!Main.dedServ)
					{
						for (int i = 0; i < 4; i++)
							ParticleHandler.SpawnParticle(new ImpactLine(Projectile.Bottom + new Vector2(Main.rand.NextFloat(45, 55) * -Projectile.direction, 0),
								-Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(1, 2.5f), Color.White, new Vector2(0.33f, Main.rand.NextFloat(1f, 1.5f)), 15));
					}
				}

				Projectile.netUpdate = true;
			}

			AiTimer++;
		}

		public override void Kill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt with { PitchVariance = 0.3f, Volume = 0.5f }, Projectile.Center);

				for (int i = 1; i < 6; ++i)
					Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Projectile.velocity, Mod.Find<ModGore>("SpiritMod/Gores/SkeletonBrute/SkeletonBruteGore" + i).Type, 1f);
			}
		}

		public override bool? CanDamage() => Projectile.frame == DAMAGEFRAME;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindNPCs.Add(index);

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(Projectile.frame);

		public override void ReceiveExtraAI(BinaryReader reader) => Projectile.frame = reader.ReadInt32();

		public override bool OnTileCollide(Vector2 oldVelocity) => false;

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDraw(Main.spriteBatch);
			Projectile.QuickDrawGlow(Main.spriteBatch, Projectile.GetAlpha(new Color(252, 3, 148)) * Math.Max(1 - AiTimer / FADEINTIME, 0));
			return false;
		}
	}
}