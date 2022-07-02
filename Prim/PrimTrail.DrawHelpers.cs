using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Effects.Stargoop;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Prim
{
	public partial class PrimTrail
	{
		public interface ITrailShader
		{
			string ShaderPass { get; }

			void ApplyShader<TTrail>(Effect effect, TTrail trail, List<Vector2> positions, string esp, float progressParam);
		}

		public class DefaultShader : ITrailShader
		{
			public string ShaderPass => "DefaultPass";

			public void ApplyShader<T>(Effect effect, T trail, List<Vector2> positions, string esp, float progressParam)
			{
				if (effect.HasParameter("vnoise"))
					effect.Parameters["vnoise"].SetValue(ModContent.Request<Texture2D>("Utilities/Noise/vnoise", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);

				if (effect.HasParameter("noiseTexture"))
					effect.Parameters["noiseTexture"].SetValue(ModContent.Request<Texture2D>("Utilities/Noise/noise", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);

				if (effect.HasParameter("spotTexture"))
					effect.Parameters["spotTexture"].SetValue(ModContent.Request<Texture2D>("Utilities/Noise/Spot", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);

				if (effect.HasParameter("ripperTexture"))
					effect.Parameters["ripperTexture"].SetValue(ModContent.Request<Texture2D>("Textures/RipperSlug", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);

				if (effect.HasParameter("arcLashColorTwo"))
					effect.Parameters["arcLashColorTwo"].SetValue(new Vector3(1.0f, 1.0f, 1.0f));
				
				try {
					effect.Parameters["progress"].SetValue(progressParam);
					effect.CurrentTechnique.Passes[esp].Apply();
					effect.CurrentTechnique.Passes[ShaderPass].Apply();
				}
				catch {
					// ignored
				}
			}
		}

		protected static Vector2 CurveNormal(List<Vector2> points, int index)
		{
			if (points.Count == 1)
				return points[0];

			if (index == 0) {
				return Clockwise90(Vector2.Normalize(points[1] - points[0]));
			}

			return Clockwise90(index == points.Count - 1
				? Vector2.Normalize(points[index] - points[index - 1]) 
				: Vector2.Normalize(points[index + 1] - points[index - 1]));
		}

		protected static Vector2 Clockwise90(Vector2 vector) => new Vector2(-vector.Y, vector.X);

		protected void PrepareShader(Effect effects, string passName, float progress, Color? color = null)
		{
			if (color == null)
				color = Color.White;

			int width = GraphicsDevice.Viewport.Width;
			int height = GraphicsDevice.Viewport.Height;

			Vector2 zoom = Main.GameViewMatrix.Zoom;
			Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) *
			              Matrix.CreateTranslation(width / 2f, height / -2f, 0) * Matrix.CreateRotationZ(MathHelper.Pi) *
			              Matrix.CreateScale(zoom.X, zoom.Y, 1f);

			Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);
			effects.Parameters["WorldViewProjection"].SetValue(view * projection);

			if (effects.HasParameter("uColor"))
				effects.Parameters["uColor"].SetValue(color.Value.ToVector3());

			TrailShader.ApplyShader(effects, this, Points, passName, progress);
		}

		protected void PrepareBasicShader()
		{
			BasicEffect basicEffect;
			if (Pixellated)
			{
				Helpers.SetBasicEffectMatrices(ref SpiritMod.primitives.pixelEffect, Main.GameViewMatrix.Zoom);
				basicEffect = SpiritMod.primitives.pixelEffect;
			}
			else if (this is IGalaxySprite)
			{
				Helpers.SetBasicEffectMatrices(ref SpiritMod.primitives.galaxyEffect, new Vector2(1));
				basicEffect = SpiritMod.primitives.galaxyEffect;
			}
			else
				basicEffect = SpiritMod.basicEffect;

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
			}
		}

		protected void AddVertex(Vector2 position, Color color, Vector2 uv)
		{
			if (CurrentIndex < Vertices.Length) 
			{
				if (Pixellated || this is IGalaxySprite)
					Vertices[CurrentIndex++] =
						new VertexPositionColorTexture(new Vector3((position - Main.screenPosition) / 2, 0f), color, uv);

				else
					Vertices[CurrentIndex++] =
						new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0f), color, uv);
			}
		}

		protected void MakePrimHelix(int index, int baseWidth, float alphaValue, Color baseColor = default,
			float fadeValue = 1, float sineFactor = 0)
		{
			float floatCap = Cap;
			Color color = (baseColor == default ? Color.White : baseColor) * (index / floatCap) * fadeValue;

			Vector2 normal = CurveNormal(Points, index);
			Vector2 normalAhead = CurveNormal(Points, index + 1);

			float fallout1 = (float) Math.Sin(index * (3.14f / floatCap));
			float fallout2 = (float) Math.Sin((index + 1) * (3.14f / floatCap));
			float lerpAmount = Counter / 15f;
			float sine1 = index * (6.14f / Points.Count);
			float sine2 = (index + 1) * (6.14f / floatCap);
			float width1 = baseWidth * Math.Abs((float) Math.Sin(sine1 + lerpAmount) * (index / floatCap)) * fallout1;
			float width2 = baseWidth * Math.Abs((float) Math.Sin(sine2 + lerpAmount) * ((index + 1) / floatCap)) * fallout2;

			Vector2 firstUp = Points[index] - normal * width1 +
			                  new Vector2(0, (float) Math.Sin(Counter / 10f + index / 3f)) * sineFactor;

			Vector2 firstDown = Points[index] + normal * width1 +
			                    new Vector2(0, (float) Math.Sin(Counter / 10f + index / 3f)) * sineFactor;

			Vector2 secondUp = Points[index + 1] - normalAhead * width2 +
			                   new Vector2(0, (float) Math.Sin(Counter / 10f + (index + 1) / 3f)) * sineFactor;

			Vector2 secondDown = Points[index + 1] + normalAhead * width2 +
			                     new Vector2(0, (float) Math.Sin(Counter / 10f + (index + 1) / 3f)) * sineFactor;

			AddVertex(firstDown, color * alphaValue, new Vector2(index / floatCap, 1));
			AddVertex(firstUp, color * alphaValue, new Vector2(index / floatCap, 0));
			AddVertex(secondDown, color * alphaValue, new Vector2((index + 1) / floatCap, 1));

			AddVertex(secondUp, color * alphaValue, new Vector2((index + 1) / floatCap, 0));
			AddVertex(secondDown, color * alphaValue, new Vector2((index + 1) / floatCap, 1));
			AddVertex(firstUp, color * alphaValue, new Vector2(index / floatCap, 0));
		}

		protected void MakePrimMidFade(int i, int baseWidth, float alphaValue, Color baseColor = default,
			float fadeValue = 1, float sineFactor = 0)
		{
			float floatCap = Cap;

			Color color = (baseColor == default ? Color.White : baseColor) * (i / floatCap) * fadeValue;

			Vector2 normal = CurveNormal(Points, i);
			Vector2 normalAhead = CurveNormal(Points, i + 1);

			float width1 = i / floatCap * baseWidth;
			float width2 = (i + 1) / floatCap * baseWidth;

			Vector2 firstUp = Points[i] - normal * width1 +
			                  new Vector2(0, (float) Math.Sin(Counter / 10f + i / 3f)) * sineFactor;

			Vector2 firstDown = Points[i] + normal * width1 +
			                    new Vector2(0, (float) Math.Sin(Counter / 10f + i / 3f)) * sineFactor;

			Vector2 secondUp = Points[i + 1] - normalAhead * width2 +
			                   new Vector2(0, (float) Math.Sin(Counter / 10f + (i + 1) / 3f)) * sineFactor;

			Vector2 secondDown = Points[i + 1] + normalAhead * width2 +
			                     new Vector2(0, (float) Math.Sin(Counter / 10f + (i + 1) / 3f)) * sineFactor;

			AddVertex(firstDown, color * alphaValue, new Vector2(i / floatCap, 1));
			AddVertex(firstUp, color * alphaValue, new Vector2(i / floatCap, 0));
			AddVertex(secondDown, color * alphaValue, new Vector2((i + 1) / floatCap, 1));

			AddVertex(secondUp, color * alphaValue, new Vector2((i + 1) / floatCap, 0));
			AddVertex(secondDown, color * alphaValue, new Vector2((i + 1) / floatCap, 1));
			AddVertex(firstUp, color * alphaValue, new Vector2(i / floatCap, 0));
		}

		protected void DrawBasicTrail(Color color, float widthVar)
		{
			//int currentIndex = 0;
			//VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[_noOfPoints];
			for (int i = 0; i < Points.Count; i++)
				if (i == 0) {

					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

					AddVertex(Points[i], color * AlphaValue,
						new Vector2((float) Math.Sin(Counter / 20f), (float) Math.Sin(Counter / 20f)));

					AddVertex(secondUp, color * AlphaValue,
						new Vector2((float) Math.Sin(Counter / 20f), (float) Math.Sin(Counter / 20f)));

					AddVertex(secondDown, color * AlphaValue,
						new Vector2((float) Math.Sin(Counter / 20f), (float) Math.Sin(Counter / 20f)));
				}
				else {
					if (i == Points.Count - 1)
						continue;

					float floatCap = Cap;

					Vector2 normal = CurveNormal(Points, i);
					Vector2 normalAhead = CurveNormal(Points, i + 1);

					float j = (floatCap + (float) Math.Sin(Counter / 10f) * 1 - i * 0.1f) / floatCap;
					widthVar *= j;

					Vector2 firstUp = Points[i] - normal * widthVar;
					Vector2 firstDown = Points[i] + normal * widthVar;
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

					AddVertex(firstDown, color * AlphaValue, new Vector2(i / floatCap, 1));
					AddVertex(firstUp, color * AlphaValue, new Vector2(i / floatCap, 0));
					AddVertex(secondDown, color * AlphaValue, new Vector2((i + 1) / floatCap, 1));

					AddVertex(secondUp, color * AlphaValue, new Vector2((i + 1) / floatCap, 0));
					AddVertex(secondDown, color * AlphaValue, new Vector2((i + 1) / floatCap, 1));
					AddVertex(firstUp, color * AlphaValue, new Vector2(i / floatCap, 0));
				}
		}
	}
}