using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;
using Terraria.ModLoader;

namespace SpiritMod.Skies
{
	public class MeteorBiomeSky2 : CustomSky
	{

		private UnifiedRandom _random = new UnifiedRandom();

		private Texture2D _beamTexture;

		private Texture2D[] _rockTextures;

		private bool skyActive;

		private float opacity;

	
        public override void Update(GameTime gameTime)
        {
            if (skyActive && opacity < 1f)
            {
                opacity += 0.01f;
            }
            else if (!skyActive && opacity > 0f)
            {
                opacity -= 0.005f;
            }
        }
        public override Color OnTileColor(Color inColor)
        {
            float amt = opacity * .3f;
            return inColor.MultiplyRGB(new Color(1f - amt, 1f - amt, 1f - amt));
        }

		public override float GetCloudAlpha()
		{
			return (1f - this.opacity) * 0.3f + 0.7f;
		}

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {

        }
        public override void Activate(Vector2 position, params object[] args)
        {
            skyActive = true;
        }
        public override void Deactivate(params object[] args)
		{
			this.skyActive = false;
		}

		public override void Reset()
		{
			this.skyActive = false;
		}

		public override bool IsActive()
		{
			if (!this.skyActive)
			{
				return this.opacity > 0.001f;
			}
			return true;
		}
	}
}
