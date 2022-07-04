using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Particles;
using SpiritMod.Prim;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.GraveyardTome
{
	public class GraveyardSkull : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Skull");
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(32, 42);
			Projectile.scale = Main.rand.NextFloat(0.4f, 0.5f);
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 255;
			Projectile.timeLeft = 120;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Color.Lerp(Color.White, Color.Red, 0.65f) * 0.1f), new RoundCap(), new SleepingStarTrailPosition(), 100 * Projectile.scale, 300);
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Color.Red, new Color(181, 0, 116)), new NoCap(), new DefaultTrailPosition(), 100 * Projectile.scale, 200, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_3").Value, 0.2f, 1f, 1f));
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Color.Red, new Color(181, 0, 116)), new NoCap(), new DefaultTrailPosition(), 100 * Projectile.scale, 200, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_3").Value, 0.2f, 1f, 1f));
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.immune[Projectile.owner] = 20;

		public struct SkullMovement
		{
			public float Amplitude { get; set; }
			public float WaveLength { get; set; }
			public int Direction { get; set; }

			public SkullMovement(float Amplitude, float WaveLength = 0)
			{
				this.Amplitude = Amplitude;
				this.WaveLength = WaveLength;
				Direction = Main.rand.NextBool() ? -1 : 1;
			}
		}

		ref float Timer => ref Projectile.ai[0];
		public SkullMovement Movement;

		private Vector2 _origVel;

		public override void AI()
		{
			Projectile.alpha = (int)MathHelper.Max(Projectile.alpha - 10, 0);
			Projectile.tileCollide = Projectile.timeLeft < 110;

			if (!Main.dedServ && Main.rand.NextFloat(2f) < Projectile.alpha/255f)
			{
				GlowParticle particle = new GlowParticle(
					Projectile.Center + Main.rand.NextVector2Circular(4, 4) * Projectile.scale,
					Projectile.velocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.4f) * (Projectile.alpha / 255f),
					Color.Red * (Projectile.Opacity/2 + 0.5f),
					Main.rand.NextFloat(0.02f, 0.04f),
					Main.rand.Next(30, 40));

				ParticleHandler.SpawnParticle(particle);
			}

			Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() / 2);

			if (++Timer == 1)
			{
				_origVel = Projectile.velocity;
				Projectile.netUpdate = true;
			}
			else
			{
				NPC npc = Projectiles.ProjectileExtras.FindNearestNPC(Projectile.Center, 150, false);
				if(npc != null)
				{
					_origVel = Vector2.Lerp(_origVel, Projectile.DirectionTo(npc.Center) * 20, 0.17f);
					Movement.Amplitude = MathHelper.Lerp(Movement.Amplitude, 0, 0.1f);
				}

				Projectile.velocity = _origVel.RotatedBy(Math.Cos(MathHelper.TwoPi * Timer / Movement.WaveLength) * MathHelper.ToRadians(Movement.Amplitude) * Movement.Direction);
				if (Timer < 30)
				{
					_origVel *= 1.03f;
					Projectile.scale *= 1.02f;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
			Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
			Projectile.rotation = Projectile.velocity.ToRotation() - ((Projectile.spriteDirection < 0) ? MathHelper.Pi : 0);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D mask = ModContent.Request<Texture2D>(Texture + "_mask", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Main.spriteBatch.Draw(mask, Projectile.Center - Main.screenPosition, Projectile.DrawFrame(), Color.White * Projectile.Opacity,
				Projectile.rotation, Projectile.DrawFrame().Size() / 2, Projectile.scale * 1.15f, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			Projectile.QuickDraw(Main.spriteBatch);
			Projectile.QuickDrawGlow(Main.spriteBatch);

			Main.spriteBatch.Draw(mask, Projectile.Center - Main.screenPosition, Projectile.DrawFrame(), Color.White * (1 - Projectile.Opacity) * Projectile.Opacity, 
				Projectile.rotation, Projectile.DrawFrame().Size() / 2, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(_origVel);
			writer.Write(Movement.Amplitude);
			writer.Write(Movement.WaveLength);
			writer.Write(Movement.Direction);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			_origVel = reader.ReadVector2();
			Movement.Amplitude = reader.ReadSingle();
			Movement.WaveLength = reader.ReadSingle();
			Movement.Direction = reader.ReadInt32();
		}

		public override void Kill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.Center);
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/skullscrem") with { PitchVariance = 0.2f, Volume = 0.7f }, Projectile.Center);
			}

			Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GraveyardBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			for (int i = 0; i <= 3; i++)
			{
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position + new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height)),
					Main.rand.NextVector2Circular(-1, 1),
					Mod.Find<ModGore>("SpiritMod/Gores/Skelet/grave" + Main.rand.Next(1, 5)).Type,
					Projectile.scale);
				gore.timeLeft = 20;
			}
		}
	}
}