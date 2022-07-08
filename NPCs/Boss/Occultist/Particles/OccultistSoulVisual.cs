using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using SpiritMod.VerletChains;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist.Particles
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
			var drawFrame = new Rectangle(0, _frame * texture.Height / 2, texture.Width, texture.Height / 2);

			Effect effect = SpiritMod.ShaderDict["PrimitiveTextureMap"];
			effect.Parameters["uTexture"].SetValue(ModContent.Request<Texture2D>("SpiritMod/NPCs/Boss/Occultist/SoulTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
			Vector2[] vertices = _chain.VerticesArray();
			vertices.IterateArray(delegate (ref Vector2 vertex, int index, float progress) { IterateVerticesSine(ref vertex, progress); });
			var strip = new PrimitiveStrip
			{
				Color = Color.White * _opacity,
				Width = 13 * Scale,
				PositionArray = vertices,
				TaperingType = StripTaperType.None,
			};
			PrimitiveRenderer.DrawPrimitiveShape(strip, effect);

			var origin = new Vector2(drawFrame.Width / 2, drawFrame.Height);
			spriteBatch.Draw(texture, _chain.StartPosition - Main.screenPosition, drawFrame, Color.White * _opacity, Rotation, origin, Scale, SpriteEffects.None, 0);

			Texture2D bloom = ModContent.Request<Texture2D>("SpiritMod/Effects/Ripple", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			spriteBatch.Draw(bloom, _chain.StartPosition - Main.screenPosition, null, Color.Red * _opacity, 0, bloom.Size() / 2, 0.75f, SpriteEffects.None, 0);
		}

		private void IterateVerticesSine(ref Vector2 vertex, float progress)
		{
			if (progress == 0)
				return;

			var DirectionUnit = Vector2.Normalize(Position - vertex);
			DirectionUnit = DirectionUnit.RotatedBy(MathHelper.PiOver2);
			float amplitude = 4f;
			float speed = 1.2f;
			float numwaves = 0.5f;
			vertex += DirectionUnit * (float)Math.Sin(speed * (Main.GameUpdateCount / 10f) + progress * MathHelper.TwoPi * numwaves) * progress * amplitude;
		}
	}
}