using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
					effect.Parameters["vnoise"].SetValue(ModContent.GetTexture("SpiritMod/Textures/vnoise"));

				if (effect.HasParameter("noiseTexture"))
					effect.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Noise/noise"));

				if (effect.HasParameter("spotTexture"))
					effect.Parameters["spotTexture"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Noise/Spot"));

				if (effect.HasParameter("ripperTexture"))
					effect.Parameters["ripperTexture"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/RipperSlug"));

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

			int width = _device.Viewport.Width;
			int height = _device.Viewport.Height;

			Vector2 zoom = Main.GameViewMatrix.Zoom;
			Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) *
			              Matrix.CreateTranslation(width / 2f, height / -2f, 0) * Matrix.CreateRotationZ(MathHelper.Pi) *
			              Matrix.CreateScale(zoom.X, zoom.Y, 1f);

			Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);
			effects.Parameters["WorldViewProjection"].SetValue(view * projection);

			if (effects.HasParameter("uColor"))
				effects.Parameters["uColor"].SetValue(color.Value.ToVector3());

			_trailShader.ApplyShader(effects, this, _points, passName, progress);
		}

		protected void PrepareBasicShader()
		{
			int width = _device.Viewport.Width;
			int height = _device.Viewport.Height;

			Vector2 zoom = Main.GameViewMatrix.Zoom;

			Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) *
			              Matrix.CreateTranslation(width / 2f, height / -2f, 0) * Matrix.CreateRotationZ(MathHelper.Pi) *
			              Matrix.CreateScale(zoom.X, zoom.Y, 1f);

			Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);

			_basicEffect.View = view;
			_basicEffect.Projection = projection;

			foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
				pass.Apply();
		}

		protected void AddVertex(Vector2 position, Color color, Vector2 uv)
		{
			if (currentIndex < vertices.Length)
				vertices[currentIndex++] =
					new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0f), color, uv);
		}

		protected void MakePrimHelix(int index, int baseWidth, float alphaValue, Color baseColor = default,
			float fadeValue = 1, float sineFactor = 0)
		{
			float floatCap = _cap;
			Color color = (baseColor == default ? Color.White : baseColor) * (index / floatCap) * fadeValue;

			Vector2 normal = CurveNormal(_points, index);
			Vector2 normalAhead = CurveNormal(_points, index + 1);

			float fallout1 = (float) Math.Sin(index * (3.14f / floatCap));
			float fallout2 = (float) Math.Sin((index + 1) * (3.14f / floatCap));
			float lerpAmount = _counter / 15f;
			float sine1 = index * (6.14f / _points.Count);
			float sine2 = (index + 1) * (6.14f / floatCap);
			float width1 = baseWidth * Math.Abs((float) Math.Sin(sine1 + lerpAmount) * (index / floatCap)) * fallout1;
			float width2 = baseWidth * Math.Abs((float) Math.Sin(sine2 + lerpAmount) * ((index + 1) / floatCap)) * fallout2;

			Vector2 firstUp = _points[index] - normal * width1 +
			                  new Vector2(0, (float) Math.Sin(_counter / 10f + index / 3f)) * sineFactor;

			Vector2 firstDown = _points[index] + normal * width1 +
			                    new Vector2(0, (float) Math.Sin(_counter / 10f + index / 3f)) * sineFactor;

			Vector2 secondUp = _points[index + 1] - normalAhead * width2 +
			                   new Vector2(0, (float) Math.Sin(_counter / 10f + (index + 1) / 3f)) * sineFactor;

			Vector2 secondDown = _points[index + 1] + normalAhead * width2 +
			                     new Vector2(0, (float) Math.Sin(_counter / 10f + (index + 1) / 3f)) * sineFactor;

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
			float floatCap = _cap;

			Color color = (baseColor == default ? Color.White : baseColor) * (i / floatCap) * fadeValue;

			Vector2 normal = CurveNormal(_points, i);
			Vector2 normalAhead = CurveNormal(_points, i + 1);

			float width1 = i / floatCap * baseWidth;
			float width2 = (i + 1) / floatCap * baseWidth;

			Vector2 firstUp = _points[i] - normal * width1 +
			                  new Vector2(0, (float) Math.Sin(_counter / 10f + i / 3f)) * sineFactor;

			Vector2 firstDown = _points[i] + normal * width1 +
			                    new Vector2(0, (float) Math.Sin(_counter / 10f + i / 3f)) * sineFactor;

			Vector2 secondUp = _points[i + 1] - normalAhead * width2 +
			                   new Vector2(0, (float) Math.Sin(_counter / 10f + (i + 1) / 3f)) * sineFactor;

			Vector2 secondDown = _points[i + 1] + normalAhead * width2 +
			                     new Vector2(0, (float) Math.Sin(_counter / 10f + (i + 1) / 3f)) * sineFactor;

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
			for (int i = 0; i < _points.Count; i++)
				if (i == 0) {

					Vector2 normalAhead = CurveNormal(_points, i + 1);
					Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

					AddVertex(_points[i], color * _alphaValue,
						new Vector2((float) Math.Sin(_counter / 20f), (float) Math.Sin(_counter / 20f)));

					AddVertex(secondUp, color * _alphaValue,
						new Vector2((float) Math.Sin(_counter / 20f), (float) Math.Sin(_counter / 20f)));

					AddVertex(secondDown, color * _alphaValue,
						new Vector2((float) Math.Sin(_counter / 20f), (float) Math.Sin(_counter / 20f)));
				}
				else {
					if (i == _points.Count - 1)
						continue;

					float floatCap = _cap;

					Vector2 normal = CurveNormal(_points, i);
					Vector2 normalAhead = CurveNormal(_points, i + 1);

					float j = (floatCap + (float) Math.Sin(_counter / 10f) * 1 - i * 0.1f) / floatCap;
					widthVar *= j;

					Vector2 firstUp = _points[i] - normal * widthVar;
					Vector2 firstDown = _points[i] + normal * widthVar;
					Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

					AddVertex(firstDown, color * _alphaValue, new Vector2(i / floatCap, 1));
					AddVertex(firstUp, color * _alphaValue, new Vector2(i / floatCap, 0));
					AddVertex(secondDown, color * _alphaValue, new Vector2((i + 1) / floatCap, 1));

					AddVertex(secondUp, color * _alphaValue, new Vector2((i + 1) / floatCap, 0));
					AddVertex(secondDown, color * _alphaValue, new Vector2((i + 1) / floatCap, 1));
					AddVertex(firstUp, color * _alphaValue, new Vector2(i / floatCap, 0));
				}
		}
	}
}