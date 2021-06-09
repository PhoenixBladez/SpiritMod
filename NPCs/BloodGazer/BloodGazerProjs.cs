using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Boss.SteamRaider;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using SpiritMod.VerletChains;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
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
			projectile.width = 10;
			projectile.height = 10;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = 1;
			projectile.timeLeft = 180;
		}

		bool primsCreated = false;
		public override void AI()
		{

			if (projectile.velocity.Length() < 22)
				projectile.velocity *= 1.02f;

			if (!primsCreated) {
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new RipperPrimTrail(projectile));
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(new LegacySoundStyle(SoundID.NPCHit, 8).WithPitchVariance(0.2f).WithVolume(0.3f), projectile.Center);
			for (int i = 0; i < 20; i++) {
				Dust.NewDustPerfect(projectile.Center, 5, Main.rand.NextFloat(0.25f, 0.5f) * projectile.velocity.RotatedBy(3.14f + Main.rand.NextFloat(-0.4f, 0.4f)));
			}
		}
	}

	public class BloodGazerEyeShotWavy : BloodGazerEyeShot
	{
		public override string Texture => "Terraria/Projectile_1";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Blood Shot");

		public override bool PreAI()
		{
			projectile.position -= projectile.velocity;
			projectile.position += projectile.velocity.RotatedBy(Math.Sin(projectile.timeLeft/6f) * MathHelper.PiOver4 * projectile.ai[0]);
			return true;
		}
	}

	public class RunicEye : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Runic Eye");

		public override void SetDefaults()
		{
			projectile.Size = Vector2.One * 10;
			projectile.tileCollide = false;
			projectile.friendly = projectile.hostile = false;
			projectile.alpha = 255;
			projectile.scale = 2f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, projectile.Center - Main.screenPosition, null, Color.Red * projectile.Opacity * 0.5f, 0, bloom.Size() / 2, new Vector2(1, 0.75f) * projectile.scale/2, SpriteEffects.None, 0);

			Texture2D tex = Main.projectileTexture[projectile.type];
			Rectangle eyeframe = new Rectangle(0, 0, tex.Width, tex.Height / 2);
			Rectangle pupilframe = new Rectangle(0, tex.Height / 2 + 1, tex.Width, tex.Height / 2);
			void DrawEye(Vector2 position, float Opacity)
			{
				spriteBatch.Draw(tex, position - Main.screenPosition, eyeframe, Color.White * Opacity, 0, eyeframe.Size() / 2, projectile.scale, SpriteEffects.None, 0);
				spriteBatch.Draw(tex, position + (Vector2.UnitX.RotatedBy(projectile.rotation) * projectile.localAI[0]) - Main.screenPosition, pupilframe, Color.White * Opacity, 
					0, eyeframe.Size() / 2, projectile.scale, SpriteEffects.None, 0);
			}
			switch (projectile.ai[0])
			{
				case 0://fade in
				case 3:
					for(int i = 0; i < 4; i++)
					{
						Vector2 pos = new Vector2(0, projectile.alpha / 3f);
						pos = pos.RotatedBy((i / 4f * MathHelper.TwoPi) + MathHelper.ToRadians(projectile.alpha * 0.2f));
						DrawEye(pos + projectile.Center, projectile.Opacity / 2);
					}
					break;
				case 1:
				case 2:
					DrawEye(projectile.Center, projectile.Opacity);
					for(int i = 0; i < 3; i++)
					{
						Vector2 pos = new Vector2(0, 5) * (float)(Math.Sin(Main.GlobalTime * 6)/4 + 0.75);
						pos = pos.RotatedBy(i / 3f * MathHelper.TwoPi);
						DrawEye(pos + projectile.Center, projectile.Opacity / 2);
					}
					if(projectile.localAI[1] < 30f)
					{
						Texture2D raytelegraph = mod.GetTexture("Textures/Medusa_Ray");
						float Opacity = Math.Min((projectile.localAI[1] + projectile.localAI[0]) / 25f, 0.5f);
						spriteBatch.Draw(raytelegraph, projectile.Center + (Vector2.UnitX.RotatedBy(projectile.rotation) * projectile.localAI[0]) - Main.screenPosition, null, Color.Red * Opacity,
							projectile.rotation, new Vector2(0, raytelegraph.Height / 2), new Vector2(24f, 0.5f * projectile.scale), SpriteEffects.None, 0);
					}
					break;
			}
		}

		public override bool CanDamage() => false;

		public NPC Parent => Main.npc[(int)projectile.ai[1]];

		public Player Target => Main.player[Parent.target];

		public virtual bool ShootProj => true;

		public override void AI()
		{
			if(Parent.type != ModContent.NPCType<BloodGazer>() || !Parent.active || Target.dead || !Target.active) {
				projectile.Kill();
				return;
			}
			switch (projectile.ai[0])
			{
				case 0://fade in
					projectile.rotation = projectile.AngleTo(Target.Center);
					projectile.alpha = (int)MathHelper.Lerp(projectile.alpha, 0, 0.06f);
					if (projectile.alpha <= 1){
						projectile.ai[0]++;
						projectile.alpha = 0;
					}
					break;
				case 1: //aim at player
					projectile.rotation = Utils.AngleLerp(projectile.rotation, projectile.AngleTo(Target.Center), 0.12f);
					projectile.localAI[0] = MathHelper.Lerp(projectile.localAI[0], 15, 0.07f);
					if(projectile.localAI[0] > 13.5f){ //move from center to edge
						projectile.localAI[0] = 15;
						projectile.ai[0]++;
					}
					break;
				case 2: //shoot projectile
					if(++projectile.localAI[1] == 30 && ShootProj)
					{
						if (Main.netMode != NetmodeID.Server)
							Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 12, 1, -1f);
						if(Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectileDirect(projectile.Center + (Vector2.UnitX.RotatedBy(projectile.rotation) * projectile.localAI[0]),
								Vector2.UnitX.RotatedBy(projectile.rotation) * 10, ModContent.ProjectileType<BrimstoneLaser>(), projectile.damage, 1f, Main.myPlayer).netUpdate = true;
					}

					if (projectile.localAI[1] == 40)
						projectile.ai[0]++;

					break;
				case 3: //fade out
					projectile.localAI[0] = MathHelper.Lerp(projectile.localAI[0], 0, 0.12f);
					projectile.alpha = (int)MathHelper.Lerp(projectile.alpha, 255, 0.06f);
					if (projectile.alpha >= 230)
						projectile.Kill();

					break;
			}
		}
	}

	public class RunicEyeBig : RunicEye
	{
		public override string Texture => "SpiritMod/NPCs/BloodGazer/RunicEye";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Runic Eye");

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.scale *= 1.5f;
		}

		public override bool ShootProj => false;

		public override bool PreAI()
		{
			projectile.Center = Target.Center - new Vector2(0, 500);
			return true;
		}
	}

	internal class BrimstoneLaser : StarLaser, ITrailProjectile
	{
		public override string Texture => "Terraria/Projectile_1";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Brimstone Laser");

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.timeLeft *= 2;
		}

		public override void AI()
		{
			projectile.ai[0]++;
		}

		new public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.Red * 0.8f), new RoundCap(), new DefaultTrailPosition(), 15f, 1950f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), Vector2.One));
			tManager.CreateTrail(projectile, new StandardColorTrail(Color.Red * 0.5f), new RoundCap(), new DefaultTrailPosition(), 30f, 1950f);
		}
	}

	public class MortarEye : ModProjectile
	{
		public override string Texture => "SpiritMod/NPCs/BloodGazer/BloodGazerEye";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Detatched Eye");

		public override void SetDefaults()
		{
			projectile.Size = Vector2.One * 30;
			projectile.hostile = true;
		}

		public override void AI()
		{
			if (projectile.localAI[0] == 0)
			{
				projectile.localAI[0]++;
				InitializeChain(Main.npc[(int)projectile.ai[0]].Center);
			}
			projectile.velocity.Y += 0.35f;

			NPC parent = Main.npc[(int)projectile.ai[0]];
			if(!parent.active || parent.type != ModContent.NPCType<BloodGazer>())
			{
				projectile.Kill();
				return;
			}
			chain.Update(Main.npc[(int)projectile.ai[0]].Center, projectile.Center);
		}

		private Chain chain;
		public void InitializeChain(Vector2 position) => chain = new Chain(ModContent.GetTexture(Texture + "_chain"), 8, 8, position, new ChainPhysics(0.9f, 0.5f, 0f), false, true); 
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (chain == null)
				return false;

			chain.Draw(spriteBatch, out float endrot, out Vector2 endpos);
			Texture2D tex = Main.projectileTexture[projectile.type];

			spriteBatch.Draw(tex, endpos - new Vector2(chain.Texture.Height / 2, -chain.Texture.Width / 2).RotatedBy(endrot) - Main.screenPosition, tex.Bounds, drawColor, endrot, tex.Bounds.Size() / 2, projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
				Main.PlaySound(SoundID.NPCDeath22, projectile.Center);

			Gore.NewGoreDirect(projectile.position, projectile.velocity / 2, mod.GetGoreSlot("Gores/Gazer/GazerEye"), 1f).timeLeft = 10;
			foreach (var segment in chain.Segments)
				Gore.NewGoreDirect(segment.Vertex2.Position, projectile.velocity / 2, mod.GetGoreSlot("Gores/Gazer/GazerChain"), 1f).timeLeft = 10;
		}
	}
}
