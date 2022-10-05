using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.AtlasDrops.AtlasPet
{
	internal class AtlasPetPart
	{
		public Asset<Texture2D> AtlasPetTex => TextureAssets.Projectile[ModContent.ProjectileType<AtlasPetProjectile>()];

		public Vector2 position;
		public int column = 0;
		public SpriteEffects effects = SpriteEffects.None;

		private int _frame = 0;
		private float _frameCounter = 0;

		public void Update()
		{
			UpdateFrame();
		}

		private void UpdateFrame()
		{
			_frameCounter += 1f;

			if (_frameCounter > 6)
			{
				_frame++;
				_frameCounter = 0;

				if (_frame > 7)
					_frame = 0;
			}
		}

		public void Draw()
		{
			var source = new Rectangle(column * 48, _frame * 38, 48, 36);
			var origin = new Vector2(48, 36) / 2f;
			var color = Lighting.GetColor(position.ToTileCoordinates());

			Main.EntitySpriteDraw(AtlasPetTex.Value, position + origin - Main.screenPosition, source, color, 0f, origin, 1f, effects, 0);
		}
	}
}
