using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace SpiritMod.Mechanics.Trails
{
	public interface ITrailShader
	{
		string ShaderPass { get; }
		void ApplyShader(Effect effect, Trail trail, List<Vector2> positions);
	}

	#region Different Trail Shader Types
	public class DefaultShader : ITrailShader
	{
		public string ShaderPass => "DefaultPass";
		public void ApplyShader(Effect effect, Trail trail, List<Vector2> positions)
		{
			effect.CurrentTechnique.Passes[ShaderPass].Apply();
		}
	}

	public class ImageShader : ITrailShader
	{
		public string ShaderPass => "BasicImagePass";

		protected Vector2 _coordMult;
		protected float _xOffset;
		protected float _yAnimSpeed;
		protected float _strength;
		private Texture2D _texture;

		public ImageShader(Texture2D image, Vector2 coordinateMultiplier, float strength = 1f, float yAnimSpeed = 0f)
		{
			_coordMult = coordinateMultiplier;
			_strength = strength;
			_yAnimSpeed = yAnimSpeed;
			_texture = image;
		}

		public ImageShader(Texture2D image, float xCoordinateMultiplier, float yCoordinateMultiplier, float strength = 1f, float yAnimSpeed = 0f) : this(image, new Vector2(xCoordinateMultiplier, yCoordinateMultiplier), strength, yAnimSpeed)
		{
		}

		public void ApplyShader(Effect effect, Trail trail, List<Vector2> positions)
		{
			_xOffset -= _coordMult.X;
			effect.Parameters["imageTexture"].SetValue(_texture);
			effect.Parameters["coordOffset"].SetValue(new Vector2(_xOffset, Main.GlobalTime * _yAnimSpeed));
			effect.Parameters["coordMultiplier"].SetValue(_coordMult);
			effect.Parameters["strength"].SetValue(_strength);
			effect.CurrentTechnique.Passes[ShaderPass].Apply();
		}
	}
	#endregion
}