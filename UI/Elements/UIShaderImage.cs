using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace SpiritMod.UI.Elements
{
	public class UIShaderImage : UIElement
	{
		private RasterizerState _overflowRaster;

		public Texture2D Texture { get; set; }
		public Effect Effect { get; set; }
		public EffectPass Pass { get; set; }
		public bool PointSample { get; set; }
		public event Action PreDraw;

		public UIShaderImage(Texture2D texture)
		{
			_overflowRaster = new RasterizerState()
			{
				CullMode = CullMode.None,
				ScissorTestEnable = true
			};
			_useImmediateMode = true;
			Texture = texture;
		}

		public override bool ContainsPoint(Vector2 point)
		{
			return false;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (Texture == null) return;

			PreDraw?.Invoke();

			CalculatedStyle dimensions = base.GetDimensions();
			Rectangle? nullable = null;
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, PointSample ? SamplerState.PointClamp : SamplerState.AnisotropicClamp, DepthStencilState.None, _overflowRaster, null, Main.UIScaleMatrix);

			if (Pass != null) Pass.Apply();
			spriteBatch.Draw(this.Texture, dimensions.ToRectangle(), nullable, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, _overflowRaster, null, Main.UIScaleMatrix);
		}
	}
}
