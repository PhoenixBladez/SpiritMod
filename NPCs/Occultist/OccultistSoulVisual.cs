using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using SpiritMod.VerletChains;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs.Occultist
{
	public class OccultistSoulVisual : Particle
	{
		private readonly int _frame;
		private readonly int _maxTime;
		private float _opacity;
		private readonly Chain _chain = null;
		public OccultistSoulVisual(Vector2 position, Vector2 velocity, float scale, int maxTime)
		{
			_frame = Main.rand.Next(2);
			Position = position;
			Scale = scale;
			Color = Color.White;
			_opacity = 1;
			Velocity = velocity;
			_chain = new Chain(8 * Scale, 5, Position, new ChainPhysics(0.5f, 0f, 0f), true, false);
			_maxTime = maxTime;
		}

		public override void Update()
		{
			_chain.Update(Position, Position);
			Rotation = Velocity.ToRotation() + MathHelper.PiOver2;
			Velocity = Velocity.RotatedByRandom(0.1f) * 0.97f;
			_opacity = (float)Math.Sin(Math.Pow((float)TimeActive / _maxTime, 2) * MathHelper.Pi) * 0.7f;
			Lighting.AddLight(Position, Color.Red.ToVector3() * 0.5f * _opacity);

			if (TimeActive > _maxTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D texture = ParticleHandler.GetTexture(Type);
			Rectangle drawFrame = new Rectangle(0, _frame * texture.Height / 2, texture.Width, texture.Height / 2);

			Effect effect = SpiritMod.ShaderDict["PrimitiveTextureMap"];
			effect.Parameters["uTexture"].SetValue(SpiritMod.instance.GetTexture("NPCs/Occultist/SoulTrail"));
			Vector2[] vertices = _chain.VerticesArray();
			IterateVerticesSine(ref vertices);
			PrimitiveStrip strip = new PrimitiveStrip
			{
				Color = Color.White * _opacity,
				Width = 13 * Scale,
				PositionArray = vertices,
				TaperingType = StripTaperType.None,
			};
			PrimitiveRenderer.DrawPrimitiveShape(strip, effect);

			Vector2 origin = new Vector2(drawFrame.Width / 2, drawFrame.Height);
			spriteBatch.Draw(texture, _chain.StartPosition - Main.screenPosition, drawFrame, Color.White * _opacity, Rotation, origin, Scale, SpriteEffects.None, 0);

			Texture2D bloom = SpiritMod.instance.GetTexture("Effects/Ripple");
			spriteBatch.Draw(bloom, _chain.StartPosition - Main.screenPosition, null, Color.Red * _opacity, 0, bloom.Size() / 2, 0.75f, SpriteEffects.None, 0);
		}

		private void IterateVerticesSine(ref Vector2[] vertices)
		{
			for (int i = 1; i < vertices.Length; i++)
			{
				Vector2 DirectionUnit = Vector2.Normalize(Position - vertices[i]);
				DirectionUnit = DirectionUnit.RotatedBy(MathHelper.PiOver2);
				float amplitude = 4f;
				float speed = 1.2f;
				float numwaves = 0.5f;
				float progress = i / (float)vertices.Length;
				vertices[i] += DirectionUnit * (float)Math.Sin((speed * Main.GameUpdateCount / 10f) + (progress * MathHelper.TwoPi * numwaves)) * progress * amplitude;
			}
		}
	}
}