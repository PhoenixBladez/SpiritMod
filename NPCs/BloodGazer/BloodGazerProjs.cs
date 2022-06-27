using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using SpiritMod.NPCs.Boss.SteamRaider;
using SpiritMod.Prim;
using SpiritMod.VerletChains;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BloodGazer
{
	public class BloodGazerEyeShot : ModProjectile
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blood Shot");

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
		}

		bool primsCreated = false;
		public override void AI()
		{

			if (Projectile.velocity.Length() < 22)
				Projectile.velocity *= 1.02f;

			if (!primsCreated && !Main.dedServ) {
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new RipperPrimTrail(Projectile));
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(new LegacySoundStyle(SoundID.NPCHit, 8).WithPitchVariance(0.2f).WithVolume(0.3f), Projectile.Center);
			for (int i = 0; i < 20; i++) {
				Dust.NewDustPerfect(Projectile.Center, 5, Main.rand.NextFloat(0.25f, 0.5f) * Projectile.velocity.RotatedBy(3.14f + Main.rand.NextFloat(-0.4f, 0.4f)));
			}
		}
	}

	public class BloodGazerEyeShotWavy : BloodGazerEyeShot
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blood Shot");

		public override bool PreAI()
		{
			Projectile.position -= Projectile.velocity;
			Projectile.position += Projectile.velocity.RotatedBy(Math.Sin(Projectile.timeLeft/6f) * MathHelper.PiOver4 * Projectile.ai[0]);
			return true;
		}
	}

	public class RunicEye : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Runic Eye");
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.Size = Vector2.One * 10;
			Projectile.tileCollide = false;
			Projectile.friendly = Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.scale = 1.5f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D pupiltex = ModContent.Request<Texture2D>(Texture + "_pupil");
			void DrawEye(Vector2 position, float Opacity)
			{
				spriteBatch.Draw(tex, position - Main.screenPosition, Projectile.DrawFrame(), Color.White * Opacity, 0, Projectile.DrawFrame().Size() / 2, Projectile.scale, SpriteEffects.None, 0);
				spriteBatch.Draw(pupiltex, position + (Vector2.UnitY * 8) + (Vector2.UnitX.RotatedBy(Projectile.rotation) * Projectile.localAI[0]) - Main.screenPosition, null, Color.White * Opacity,
					0, pupiltex.Size() / 2, Projectile.scale * 0.75f, SpriteEffects.None, 0);
			}

			switch (Projectile.ai[0])
			{
				case 0://fade in
				case 3:
					for (int i = 0; i < 4; i++)
					{
						Vector2 pos = new Vector2(0, Projectile.alpha / 3f);
						pos = pos.RotatedBy((i / 4f * MathHelper.TwoPi) + MathHelper.ToRadians(Projectile.alpha * 0.2f));
						DrawEye(pos + Projectile.Center, Projectile.Opacity / 2);
					}
					break;
				case 1:
				case 2:
					DrawEye(Projectile.Center, Projectile.Opacity);
					for (int i = 0; i < 3; i++)
					{
						Vector2 pos = new Vector2(0, 5) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 6) / 4 + 0.75);
						pos = pos.RotatedBy(i / 3f * MathHelper.TwoPi);
						DrawEye(pos + Projectile.Center, Projectile.Opacity / 2);
					}
					if (Projectile.localAI[1] < 30f)
					{
						Texture2D raytelegraph = Mod.GetTexture("Textures/Medusa_Ray");
						float Opacity = Math.Min((Projectile.localAI[1] + Projectile.localAI[0]) / 25f, 0.5f);
						spriteBatch.Draw(raytelegraph, Projectile.Center + (Vector2.UnitX.RotatedBy(Projectile.rotation) * Projectile.localAI[0]) - Main.screenPosition, null, Color.Red * Opacity,
							Projectile.rotation, new Vector2(0, raytelegraph.Height / 2), new Vector2(24f, 0.5f * Projectile.scale), SpriteEffects.None, 0);
					}
					break;
			}
			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D bloom = Mod.GetTexture("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, Projectile.Center - Main.screenPosition, null, Color.Red * Projectile.Opacity * 0.5f, 0, bloom.Size() / 2, new Vector2(1, 0.75f) * Projectile.scale / 2, SpriteEffects.None, 0);
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => false;

		public NPC Parent => Main.npc[(int)Projectile.ai[1]];

		public Player Target => Main.player[Parent.target];

		public virtual bool ShootProj => true;

		public override void AI()
		{
			if(Parent.type != ModContent.NPCType<BloodGazer>() || !Parent.active || Target.dead || !Target.active) {
				Projectile.Kill();
				return;
			}

			Projectile.frameCounter++;
			if(Projectile.frameCounter > 10)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}

			switch (Projectile.ai[0])
			{
				case 0://fade in
					Projectile.rotation = Projectile.AngleTo(Target.Center);
					Projectile.alpha = (int)MathHelper.Lerp(Projectile.alpha, 0, 0.06f);
					if (Projectile.alpha <= 1){
						Projectile.ai[0]++;
						Projectile.alpha = 0;
					}
					break;
				case 1: //aim at player
					Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.AngleTo(Target.Center), 0.12f);
					Projectile.localAI[0] = MathHelper.Lerp(Projectile.localAI[0], 10f, 0.07f);
					if(Projectile.localAI[0] > 7.5){ //move from center to edge
						Projectile.localAI[0] = 10f;
						Projectile.ai[0]++;
					}
					break;
				case 2: //shoot projectile
					if(++Projectile.localAI[1] == 30 && ShootProj)
					{
						if (Main.netMode != NetmodeID.Server)
							SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 12, 1, -1f);
						if(Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectileDirect(Projectile.Center + (Vector2.UnitX.RotatedBy(Projectile.rotation) * Projectile.localAI[0]),
								Vector2.UnitX.RotatedBy(Projectile.rotation) * 10, ModContent.ProjectileType<BrimstoneLaser>(), Projectile.damage, 1f, Main.myPlayer).netUpdate = true;
					}

					if (Projectile.localAI[1] == 40)
						Projectile.ai[0]++;

					break;
				case 3: //fade out
					Projectile.localAI[0] = MathHelper.Lerp(Projectile.localAI[0], 0, 0.12f);
					Projectile.alpha = (int)MathHelper.Lerp(Projectile.alpha, 255, 0.06f);
					if (Projectile.alpha >= 230)
						Projectile.Kill();

					break;
			}
		}
	}

	internal class BrimstoneLaser : StarLaser, ITrailProjectile
	{
		public override string Texture => "Terraria/Projectile_1";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Brimstone Laser");

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft *= 2;
		}

		public override void AI()
		{
			Projectile.ai[0]++;
		}

		new public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.Red * 0.8f), new RoundCap(), new DefaultTrailPosition(), 15f, 1950f, new ImageShader(Mod.GetTexture("Textures/Trails/Trail_2"), Vector2.One));
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.Red * 0.5f), new RoundCap(), new DefaultTrailPosition(), 30f, 1950f);
		}
	}

	public class MortarEye : ModProjectile
	{
		public override string Texture => "SpiritMod/NPCs/BloodGazer/BloodGazerEye";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Detatched Eye");

		public override void SetDefaults()
		{
			Projectile.Size = Vector2.One * 30;
			Projectile.hostile = true;
		}

		public override void AI()
		{
			if (Projectile.localAI[0] == 0)
			{
				Projectile.localAI[0]++;
				InitializeChain(Main.npc[(int)Projectile.ai[0]].Center);
			}
			Projectile.velocity.Y += 0.35f;

			NPC parent = Main.npc[(int)Projectile.ai[0]];
			if(!parent.active || parent.type != ModContent.NPCType<BloodGazer>())
			{
				Projectile.Kill();
				return;
			}
			chain.Update(Main.npc[(int)Projectile.ai[0]].Center, Projectile.Center);
		}

		private Chain chain;
		public void InitializeChain(Vector2 position) => chain = new Chain(8, 8, position, new ChainPhysics(0.9f, 0.5f, 0f), false, true); 
		public override bool PreDraw(ref Color lightColor)
		{
			if (chain == null)
				return false;

			Texture2D chaintex = ModContent.Request<Texture2D>(Texture + "_chain");
			chain.Draw(spriteBatch, chaintex);
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

			spriteBatch.Draw(tex, chain.EndPosition - new Vector2(chaintex.Height / 2, -chaintex.Width / 2).RotatedBy(chain.EndRotation) - Main.screenPosition, tex.Bounds, drawColor, chain.EndRotation, tex.Bounds.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
				SoundEngine.PlaySound(SoundID.NPCDeath22, Projectile.Center);

			Gore.NewGoreDirect(Projectile.position, Projectile.velocity / 2, Mod.Find<ModGore>("Gores/Gazer/GazerEye").Type, 1f).timeLeft = 10;
			foreach (var segment in chain.Segments)
				Gore.NewGoreDirect(segment.Vertex2.Position, Projectile.velocity / 2, Mod.Find<ModGore>("Gores/Gazer/GazerChain").Type, 1f).timeLeft = 10;
		}
	}
}
