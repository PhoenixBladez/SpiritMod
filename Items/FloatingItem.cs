using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items
{
	public abstract class FloatingItem : ModItem
	{
		public virtual float Bouyancy => -0.1f;
		public virtual float Weight => 0.1f;

		private int floatingTimer = 0;

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (item.wet)
			{
				gravity = Bouyancy;

				Point tilePos = (item.position + new Vector2(0, 2 - item.velocity.Y)).ToTileCoordinates();
				if (Framing.GetTileSafely(tilePos.X, tilePos.Y - 1).liquid < 100)
				{
					gravity = 0;
					item.velocity.Y *= 0.9f;
				}
			}
			else
			{
				gravity = Weight;
				floatingTimer = 0;
			}
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Point tilePos = (item.position + new Vector2(0, 2 - item.velocity.Y)).ToTileCoordinates();
			if (Framing.GetTileSafely(tilePos.X, tilePos.Y - 1).liquid < 100 && item.wet && item.velocity.LengthSquared() < 0.1f)
			{
				Vector2 drawPos = new Vector2(item.position.X - (item.width / 2f), item.Top.Y - item.height) - Main.screenPosition - (Vector2.UnitY * (float)(Math.Cos(floatingTimer++ * 0.04f) * 3f));
				spriteBatch.Draw(Main.itemTexture[item.type], drawPos, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
				return false;
			}
			return true;
		}
	}
}
