using Microsoft.Xna.Framework;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.GraveyardTome
{
	public class GraveyardPortalParticle : Particle
	{
		private float opacity;
		public int MaxTime;

		private Projectile proj;
		private Vector2 offset;

		private int FadeTime => 30;

		private float FadeRate => 1f / FadeTime;

		private float MinimumOpacity => 0.5f;

		public override bool UseAdditiveBlend => true;

		public GraveyardPortalParticle(Projectile projectile, Vector2 position, Vector2 velocity, float scale, int maxTime)
		{
			proj = projectile;
			offset = position;
			Velocity = velocity;
			Scale = scale;
			MaxTime = maxTime;
		}

		public override void Update()
		{
			if (TimeActive >= MaxTime)
				opacity -= FadeRate;
			else if (TimeActive < FadeTime && opacity < MinimumOpacity)
				opacity += FadeRate;
			else
				opacity = MinimumOpacity + (float)Math.Sin((TimeActive - FadeTime) / 30f) * 0.3f;

			Color = new Color(255, 0, 0) * opacity * (proj.scale / GraveyardPortal.MaxScale);
			Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);

			Position = proj.Center + new Vector2(proj.ai[0] * 3, 0).RotatedBy(proj.rotation) + offset;
			Velocity = Vector2.Lerp(Velocity, 0.75f * Vector2.Normalize(proj.Center + new Vector2(proj.ai[0] * 3, 0).RotatedBy(proj.rotation) - Position), 0.04f);
			offset += Velocity;

			if (opacity <= 0f || !proj.active || proj.type != ModContent.ProjectileType<GraveyardPortal>())
				Kill();
		}
	}
}
