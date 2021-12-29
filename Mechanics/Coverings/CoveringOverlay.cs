using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.Graphics.Effects;

namespace SpiritMod.Mechanics.Coverings
{
    public class CoveringOverlay : Overlay
    {
        public CoveringOverlay(EffectPriority priority, RenderLayers layer) : base(priority, layer)
        {
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            this.Mode = OverlayMode.Active;
        }

        public override void Deactivate(params object[] args)
        {
            this.Mode = OverlayMode.Inactive;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (SpiritMod.Coverings == null) return;

			SpiritMod.Coverings.Draw(spriteBatch, Layer);
        }

        public override bool IsVisible() => true;

        public override void Update(GameTime gameTime)
        {
            if (SpiritMod.Coverings == null) return;

			SpiritMod.Coverings.Update(gameTime);
        }
    }
}
