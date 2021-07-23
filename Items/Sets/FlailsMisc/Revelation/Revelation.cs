using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using SpiritMod.Utilities;
using Terraria.ModLoader;
using SpiritMod.Prim;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.FlailsMisc.Revelation
{
	public class Revelation : BaseFlailItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Revelation");
			Tooltip.SetDefault("Update description later");
		}

		public override void SafeSetDefaults()
		{
			item.Size = new Vector2(34, 30);
			item.damage = 60;
			item.rare = ItemRarityID.Green;
			item.useTime = 30;
			item.useAnimation = 30;
			item.shoot = ModContent.ProjectileType<RevelationProj>();
			item.shootSpeed = 16;
			item.knockBack = 4;
		}
	}
	public class RevelationProj : BaseFlailProj, ITrailProjectile
	{
		public RevelationProj() : base(new Vector2(0.7f, 1.3f), new Vector2(0.5f, 3f), 2, 70, 8) { }

		public override void SetStaticDefaults() => DisplayName.SetDefault("Revelation");

		public override void SpinExtras(Player player)
		{
			if (projectile.localAI[0] == 0)
			{
				SpiritMod.primitives.CreateTrail(new RevelationPrimTrailTwo(projectile, Color.White, 6, 5, 0.8f));
			}
			if (++projectile.localAI[0] % 40 == 0)
			{
				Projectile.NewProjectile(projectile.Center, Main.rand.NextVector2Circular(10, 10) + (Main.player[projectile.owner].velocity / 5), ModContent.ProjectileType<RevelationSoulWeak>(), projectile.damage, 0, projectile.owner);
				RevelationParticle particle = new RevelationParticle(
					projectile.Center,
					Vector2.Zero,
					Color.White,
					new Color(255, 94, 206),
					0.7f,
					20);
				ParticleHandler.SpawnParticle(particle);
			}

		}

		public override void NotSpinningExtras(Player player)
		{
			projectile.localAI[0]+= .5f;
			//CreateRotatingDust();
		}

		public void CreateRotatingDust()
        {
			Vector2 center = projectile.Center;
			float num4 = 2.094395f;
			for (int index1 = 0; index1 < 2; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, 258, 0.0f, 0f, 100, new Color(), .85f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = projectile.velocity;
				Main.dust[index2].noLight = true;
				Main.dust[index2].fadeIn = .85f;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)projectile.localAI[0] / 60f * 6.28318548202515 + (double)num4 * (double)index1)).ToRotationVector2() * projectile.height;
			}
			for (int index1 = 0; index1 < 2; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, 258, 0.0f, 0f, 100, new Color(), .75f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = projectile.velocity;
				Main.dust[index2].noLight = true;
				Main.dust[index2].fadeIn = .85f;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)projectile.localAI[0] / 60f * -6.28318548202515 + (double)num4 / 2 * (double)index1)).ToRotationVector2() * projectile.height / 1.2f;
			}
			for (int index1 = 0; index1 < 2; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, 258, 0.0f, 0f, 100, new Color(), 1.3f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = projectile.velocity;
				Main.dust[index2].noLight = true;
				Main.dust[index2].fadeIn = .85f;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)projectile.localAI[0] / 60f * 6.28318548202515 + (double)num4 / 4 * (double)index1)).ToRotationVector2() * projectile.height / 1.4f;
			}
		}
		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 94, 206), new Color(255, 48, 176)), new RoundCap(), new DefaultTrailPosition(), 6f, 150f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(projectile, new GradientTrail(Color.HotPink * .6f, Color.HotPink * .12f), new RoundCap(), new DefaultTrailPosition(), 90f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));

		}
	}
	public class RevelationSoulWeak : ModProjectile, ITrailProjectile
	{
		public virtual int Range => 300;

		public virtual int Duration => 90;

		public virtual int Speed => 10;

		protected Player Player => Main.player[projectile.owner];

		protected bool PrimsCreated = false;
		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.melee = true;
			projectile.Size = new Vector2(12, 12);
			projectile.tileCollide = false;
			projectile.timeLeft = Duration;
			projectile.penetrate = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void AI()
		{
			if (!PrimsCreated)
			{
				PrimsCreated = true;
				SpiritMod.primitives.CreateTrail(new RevelationPrimTrail(projectile));
			}
			NPC npc = Projectiles.ProjectileExtras.FindNearestNPC(projectile.Center, Range, true);
			if (npc != null)
			{
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(npc.Center) * Speed, 0.1f);
			}
			else
			{
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Player.Center) * Speed, 0.02f);
			}
			projectile.rotation = projectile.velocity.ToRotation();
		}
		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 258, 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}
		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 94, 206), new Color(255, 48, 176)), new RoundCap(), new DefaultTrailPosition(), 3f, 150f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(projectile, new GradientTrail(Color.HotPink * .6f, Color.HotPink * .12f), new RoundCap(), new DefaultTrailPosition(), 90f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));

		}
	}
}
