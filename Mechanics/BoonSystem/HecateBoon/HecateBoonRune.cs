using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Utilities;

namespace SpiritMod.Mechanics.BoonSystem.HecateBoon
{
	public class HecateBoonRune : ModProjectile, ITrailProjectile
	{
		const float ACCELERATION = 0.001f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune of Hecate");
			Main.projFrames[projectile.type] = 6;
		}
		bool initialized = false;

		float rotation;

		float speed = 0.05f;

		float radius = 100;

		NPC parent => Main.npc[(int)projectile.ai[0]];


		public override void SetDefaults()
		{
			projectile.width = projectile.height = 18;
			projectile.penetrate = -1;
			projectile.alpha = 0;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.frame = Main.rand.Next(6);
		}
		public override void AI()
		{
			if (!parent.active || parent.life <= 0)
				projectile.active = false;
			if (!initialized)
				rotation = projectile.ai[1] * 2.09f;
			initialized = true;
			rotation += speed;
			if (speed > 0.25f)
			{
				radius -= 1;
				if (radius < 20)
				{
					if (Main.player[parent.target] == null)
					{
						projectile.active = false;
						return;
					}
					if (projectile.ai[1] == 0)
						Projectile.NewProjectile(parent.Center, parent.DirectionTo(Main.player[parent.target].Center) * 15, ModContent.ProjectileType<HecateBoonProj>(), Main.expertMode ? (int)(parent.damage / 4) : parent.damage, 4, parent.target);
					projectile.active = false;
				}
			}
			speed += ACCELERATION;
			Vector2 offset = Vector2.UnitX.RotatedBy(rotation) * radius;
			projectile.Center = parent.Center + offset;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(Color.Purple, Color.Pink), new RoundCap(), new DefaultTrailPosition(), 8f, 200f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
		}


		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
