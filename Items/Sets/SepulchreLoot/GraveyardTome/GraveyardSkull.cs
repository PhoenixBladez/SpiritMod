using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.GraveyardTome
{
	public class GraveyardSkull : ModProjectile, ITrailProjectile
	{
		public override string Texture => "SpiritMod/Items/Sets/SepulchreLoot/ScreamingTome/ScreamingSkull";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Skull");
			Main.projFrames[projectile.type] = 6;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(32, 42);
			projectile.scale = Main.rand.NextFloat(0.4f, 0.5f);
			projectile.friendly = true;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.alpha = 255;
			projectile.timeLeft = 120;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new ProjectileOpacityTrail(projectile, Color.LimeGreen), new NoCap(), new DefaultTrailPosition(), 40 * projectile.scale, 150, new ImageShader(mod.GetTexture("Textures/vnoise"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new ProjectileOpacityTrail(projectile, Color.Lerp(Color.White, Color.LimeGreen, 0.5f) * 0.2f), new RoundCap(), new SleepingStarTrailPosition(), 100 * projectile.scale, 300);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.immune[projectile.owner] = 10;

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

		ref float Timer => ref projectile.ai[0];
		public SkullMovement Movement;

		private Vector2 _origVel;

		public override void AI()
		{
			projectile.alpha = (int)MathHelper.Max(projectile.alpha - 10, 0);

			if (!Main.dedServ && Main.rand.NextFloat(2.55f) < projectile.alpha/100f)
			{
				GlowParticle particle = new GlowParticle(
					projectile.Center + Main.rand.NextVector2Circular(4, 4),
					projectile.velocity.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(0.25f),
					Color.LimeGreen * (projectile.Opacity/2 + 0.5f),
					Main.rand.NextFloat(0.03f, 0.05f),
					Main.rand.Next(30, 40));

				ParticleHandler.SpawnParticle(particle);
			}

			Lighting.AddLight(projectile.Center, Color.LimeGreen.ToVector3() / 2);

			if (++Timer == 1)
			{
				_origVel = projectile.velocity;
				projectile.netUpdate = true;
			}
			else
			{
				NPC npc = Projectiles.ProjectileExtras.FindNearestNPC(projectile.Center, 150, false);
				if(npc != null)
					_origVel = Vector2.Lerp(_origVel, projectile.DirectionTo(npc.Center) * 24, 0.17f);

				projectile.velocity = _origVel.RotatedBy(Math.Cos(MathHelper.TwoPi * Timer / Movement.WaveLength) * MathHelper.ToRadians(Movement.Amplitude) * Movement.Direction);
				if (Timer < 40)
				{
					_origVel *= 1.025f;
					projectile.scale *= 1.025f;
				}
			}

			projectile.frame = 5;
			projectile.spriteDirection = Math.Sign(projectile.velocity.X);
			projectile.rotation = projectile.velocity.ToRotation() - ((projectile.spriteDirection < 0) ? MathHelper.Pi : 0);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D mask = mod.GetTexture("Items/Sets/SepulchreLoot/GraveyardTome/GraveyardSkullMask");
			spriteBatch.Draw(mask, projectile.Center - Main.screenPosition, projectile.DrawFrame(), Color.White * projectile.Opacity,
				projectile.rotation, projectile.DrawFrame().Size() / 2, projectile.scale * 1.15f, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			projectile.QuickDraw(spriteBatch);
			projectile.QuickDrawGlow(spriteBatch);

			spriteBatch.Draw(mask, projectile.Center - Main.screenPosition, projectile.DrawFrame(), Color.White * (1 - projectile.Opacity) * projectile.Opacity, 
				projectile.rotation, projectile.DrawFrame().Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
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
			Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 3, 1f, 0f);

			Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ModContent.ProjectileType<GraveyardBoom>(), projectile.damage, projectile.knockBack, projectile.owner);
			for (int i = 0; i <= 3; i++)
			{
				Gore gore = Gore.NewGoreDirect(projectile.position + new Vector2(Main.rand.Next(projectile.width), Main.rand.Next(projectile.height)),
					Main.rand.NextVector2Circular(-1, 1),
					mod.GetGoreSlot("Gores/Skelet/bonger" + Main.rand.Next(1, 5)),
					projectile.scale);
				gore.timeLeft = 20;
			}
		}
	}
}