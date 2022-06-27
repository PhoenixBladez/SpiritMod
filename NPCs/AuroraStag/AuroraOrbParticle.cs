using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.AuroraStag
{
	public class AuroraOrbParticle : Particle
	{
		private Color glowColor;
		private readonly NPC Parent;
		private bool returning = false;
		private float opacity = 0;

		public override bool UseAdditiveBlend => true;

		public AuroraOrbParticle(NPC parent, Vector2 position, Vector2 velocity, Color color, float scale)
		{
			Parent = parent;
			Position = position; 
			Velocity = velocity;
			glowColor = color;
			Scale = scale;
		}

		public override void Update()
		{
			if (Parent.type != NPCType<AuroraStag>() || !Parent.active || returning && Parent.Hitbox.Intersects(new Rectangle((int)Position.X, (int)Position.Y, 1, 1)))
			{
				Kill();
				return;
			}

			bool anticipation = false;
			AuroraStag stag = Parent.ModNPC as AuroraStag;
			if (stag.TameAnimationTimer < AuroraStag.ParticleReturnTime)
				returning = true;
			else if (stag.TameAnimationTimer < AuroraStag.ParticleAnticipationTime)
				anticipation = true;

			opacity = Math.Min(opacity + 0.05f, 1);
			Color = glowColor * opacity;

			if (anticipation)
			{
				Velocity = Parent.DirectionTo(Position) * 1.25f;
				return;
			}
			if (!returning && TimeActive > 26)
			{
				Velocity = Velocity.Length() * 0.987f * Vector2.Normalize(Vector2.Lerp(Velocity, Parent.DirectionFrom(Position) * 5, 0.04f));
				return;
			}
			else if (returning)
				Velocity = Vector2.Lerp(Velocity, Parent.DirectionFrom(Position) * 28, 0.06f);
		}

		public override bool UseCustomDraw => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			Vector2 newscale = new Vector2(MathHelper.Clamp(Velocity.Length() / 7.5f, 1f, 2f), 1/ MathHelper.Clamp(Velocity.Length() / 7.5f, 1f, 2f));
			spriteBatch.Draw(tex, Position - Main.screenPosition, null, Color, Velocity.ToRotation(), tex.Size() / 2, Scale * newscale, SpriteEffects.None, 0);
		}
	}
}
