using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using SpiritMod.Particles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using System.IO;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
    public class StarfireProj : ModProjectile, IDrawAdditive
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire");
            ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		private const int MaxTimeLeft = 240;
        public override void SetDefaults()
        {
			projectile.Size = new Vector2(20, 20);
            projectile.scale = Main.rand.NextFloat(0.8f, 1.3f);
            projectile.friendly = true;
            projectile.magic = true;
			projectile.timeLeft = MaxTimeLeft;
			projectile.alpha = 255;
			projectile.penetrate = 2;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}

		private Vector2 OrigVel;
		private ref float AiState => ref projectile.ai[0];
		private const float JustSpawned = 0;
		private const float CosWave = 1;
		private const float Circling = 2;
		private const float HomingAim = 3;
		private const float HomingAccelerate = 4;
		private const float FadeOut = 5;

		private const float FADEOUTTIME = 20;

		private ref float Direction => ref projectile.ai[1];

		private ref float Timer => ref projectile.localAI[0];

		private NPC Target;

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, SpiritMod.StarjinxColor(Main.GlobalTime - 1).ToVector3() / 3);
			projectile.tileCollide = Timer > 20;
			void TargetCheck()
			{
				int maxDist = 1000;
				foreach(NPC npc in Main.npc.Where(x => x.Distance(projectile.Center) < maxDist && x.active && x.CanBeChasedBy(this) && x != null)){
					StarfireLampPlayer player = Main.player[projectile.owner].GetModPlayer<StarfireLampPlayer>();
					if (player.LampTargetNPC == npc && npc.active && npc != null && npc.CanBeChasedBy(this))
					{
						AiState = HomingAim;
						Target = npc;
						projectile.netUpdate = true;
						Timer = 0;
					}
				}
			}

			switch (AiState)
			{
				case JustSpawned:
					Direction = Main.rand.NextBool() ? -1 : 1;
					OrigVel = projectile.velocity;
					AiState = Main.rand.NextBool(3) ? Circling : CosWave;
					break;
				case CosWave:
					++Timer;
					projectile.velocity = OrigVel.RotatedBy(Math.Cos(Timer / 60 * MathHelper.TwoPi) * Direction * MathHelper.Pi / 8);
					if(Timer > 10)
						TargetCheck();
					break;
				case Circling:
					++Timer;
					projectile.velocity = (OrigVel.RotatedBy(MathHelper.ToRadians(Timer * Direction * 7) + MathHelper.PiOver2 * Direction) * 0.5f) + (OrigVel * 0.33f);
					if(Timer > 10)
						TargetCheck();
					break;
				case HomingAim:
					++Timer;
					if (Timer > 8 || Target == null || !Target.active || !Target.CanBeChasedBy(this))
					{
						AiState = HomingAccelerate;
						break;
					}
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * 3, 0.2f);
					break;
				case HomingAccelerate:
					if (projectile.velocity.Length() < 24)
						projectile.velocity *= 1.05f;

					if (Target != null && Target.active && Target.CanBeChasedBy(this))
						projectile.velocity = projectile.velocity.Length() * 
							Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * projectile.velocity.Length(), 0.1f));
					break;
				case FadeOut:
					projectile.alpha += (int)(255 / FADEOUTTIME);
					break;
			}

			if(projectile.timeLeft <= FADEOUTTIME && AiState != FadeOut)
			{
				projectile.timeLeft = (int)FADEOUTTIME;
				AiState = FadeOut;
				projectile.netUpdate = true;
			}

			if(AiState != FadeOut)
			{
				projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
				projectile.alpha = Math.Max(projectile.alpha - 15, 0);
			}

			if (projectile.frameCounter++ % 5 == 0)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
			}

			if(!Main.dedServ && AiState != FadeOut)
			{
				if (Main.rand.NextBool(12))
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(0.75f),
						Yellow, Orange, Main.rand.NextFloat(0.25f, 0.3f), 25, delegate (Particle p)
						{
							p.Velocity *= 0.93f;
						}));

				if (Main.rand.NextBool(8))
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(0.75f),
						Main.rand.NextBool(3) ? Orange : Yellow, Main.rand.NextFloat(0.15f, 0.2f), 20));
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => OnCollision();

		public override void OnHitPlayer(Player target, int damage, bool crit) => OnCollision();

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			OnCollision();
			return false;
		}

		public override bool CanDamage() => AiState != FadeOut;

		private void OnCollision()
		{
			if (AiState != FadeOut)
			{
				projectile.rotation = projectile.oldVelocity.ToRotation() - MathHelper.PiOver2;
				projectile.velocity = Vector2.Zero;
				AiState = FadeOut;
				projectile.netUpdate = true;
			}

			if (Main.dedServ)
				return;

			for (int i = 0; i < 4; i++)
			{
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.oldVelocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.2f, 0.4f), Color.White,
					SpiritMod.StarjinxColor(Main.GlobalTime - 1), Main.rand.NextFloat(0.2f, 0.4f), 25));
			}

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithPitchVariance(0.2f).WithVolume(0.3f), projectile.Center);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Target == null ? -1 : Target.whoAmI);
			writer.Write(Timer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			int whoami = reader.ReadInt32();
			Target = whoami == -1 ? null : Main.npc[whoami];
			Timer = reader.ReadSingle();
		}

		//Copypaste code from stellanova starfire, cut down on boilerplate later
		private readonly Color Yellow = new Color(242, 240, 134);
		private readonly Color Orange = new Color(255, 98, 74);
		private readonly Color Purple = new Color(255, 0, 144);
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			//set the parameters for the shader
			Effect effect = mod.GetEffect("Effects/FlameTrail");
			effect.Parameters["uTexture"].SetValue(mod.GetTexture("Textures/Trails/Trail_3"));
			effect.Parameters["uTexture2"].SetValue(mod.GetTexture("Textures/Trails/Trail_4"));
			effect.Parameters["Progress"].SetValue(Main.GlobalTime * -0.5f);
			effect.Parameters["xMod"].SetValue(1.5f);
			effect.Parameters["StartColor"].SetValue(Yellow.ToVector4());
			effect.Parameters["MidColor"].SetValue(Orange.ToVector4());
			effect.Parameters["EndColor"].SetValue(Purple.ToVector4());

			//draw the strip with the given array

			///Just using the oldpos array with no changes bugs out due to values defaulting to (0, 0) in the world until set, setting all values to projectile.position makes a weird rectangle shape as
			///the projectile spawns in. This solution isn't ideal, as it may bug out when the projectile actually passes through (0, 0), but it works well enough for now
			Vector2[] trimmedOldPos = projectile.oldPos.Where(x => x != Vector2.Zero).ToArray();
			Vector2[] posarray = new Vector2[trimmedOldPos.Length];
			trimmedOldPos.CopyTo(posarray, 0);
			posarray.IterateArray(delegate (ref Vector2 vec, int index, float progress) { vec += projectile.Size / 2; });
			var strip = new PrimitiveStrip
			{
				Color = Color.White * (float)Math.Pow(projectile.Opacity, 2),
				Width = 16 * projectile.scale,
				PositionArray = posarray,
				TaperingType = StripTaperType.None,
				WidthDelegate = delegate (float progress) { return ((float)Math.Sin((Main.GlobalTime - progress) * MathHelper.TwoPi * 1.5f) * 0.33f + 1.33f) * (float)Math.Pow(1 - progress, 0.8f); }
			};
			PrimitiveRenderer.DrawPrimitiveShape(strip, effect);

			projectile.QuickDraw(spriteBatch, drawColor: Color.White * projectile.Opacity);

			Texture2D tex = Main.extraTexture[89];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 8f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.25f, 2f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.25f, 4f * projectile.Opacity) * Timer;
			Color blurcolor = Color.Lerp(Yellow, Color.White, 0.33f) * projectile.Opacity;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
	}
}