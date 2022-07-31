using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Utilities;
using System.IO;
using SpiritMod.Particles;

namespace SpiritMod.Mechanics.BoonSystem.HecateBoon
{
	public class HecateBoonRune : ModProjectile, IDrawAdditive
	{
		private const float ACCELERATION = 0.0008f;
		private const float MAX_SPEED = 0.36f;
		private const float MIN_RADIUS = 15;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune of Hecate");
			Main.projFrames[Projectile.type] = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 18;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.frame = Main.rand.Next(6);
			Projectile.hide = true;
			Projectile.ignoreWater = true;
		}

		private bool initialized = false;

		private float rotation;

		private float speed = 0.03f;

		private float radius = 100;

		private NPC Parent => Main.npc[(int)Projectile.ai[0]];
		private Player Target => Main.player[Parent.target];
		private int RuneNumber => (int)Projectile.ai[1];

		public override void AI()
		{
			//Die if parent dies or despawns
			if (!Parent.active || Parent.life <= 0)
				Projectile.active = false;

			//Evenly spread out the rotations of all 3 runes
			if (!initialized)
			{
				rotation = RuneNumber * MathHelper.TwoPi / 3;
				Projectile.netUpdate = true;
				initialized = true;
			}

			Projectile.alpha = Math.Max(Projectile.alpha - 5, 0);
			rotation += speed;
			speed = Math.Min(speed + ACCELERATION, MAX_SPEED);

			//Start decreasing in radius when speed is high enough
			if (speed > MAX_SPEED / 4)
			{
				radius -= 1.5f;
				if (radius <= MIN_RADIUS)
				{
					//Dont shoot a projectile if the target no longer exists
					bool playerUntargettable = (Target == null || Target.dead || !Target.active);

					//Only make one projectile shot directly at the player, by checking if it's the first rune that was spawned in
					if (RuneNumber == 0 && !playerUntargettable)
					{
						Projectile p = Projectile.NewProjectileDirect(Parent.GetSource_FromAI("Boon"), Parent.Center, Vector2.Zero, ModContent.ProjectileType<HecateBoonProj>(), 
							NPCUtils.ToActualDamage(Parent.damage), 1, Main.myPlayer, Parent.target);

						if (!Main.dedServ)
						{
							SoundEngine.PlaySound(SoundID.MaxMana, Parent.Center);
							ParticleHandler.SpawnParticle(new HecateSpawnParticle(p, new Color(255, 106, 250), 0.5f, 40));
						}
					}

					Projectile.Kill();
				}
			}

			Vector2 offset = Vector2.UnitX.RotatedBy(rotation) * radius;
			Projectile.Center = Parent.Center + offset;
		}

		public void AdditiveCall(SpriteBatch sB, Vector2 screenPos)
		{
			Projectile.QuickDrawTrail(sB, drawColor: Color.White);
			Projectile.QuickDraw(sB, drawColor: Color.White);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(initialized);
			writer.Write(rotation);
			writer.Write(speed);
			writer.Write(radius);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			initialized = reader.ReadBoolean();
			rotation = reader.ReadSingle();
			speed = reader.ReadSingle();
			radius = reader.ReadSingle();
		}
	}
}
