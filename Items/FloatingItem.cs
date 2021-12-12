using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items
{
	public abstract class FloatingItem : ModItem
	{
		public virtual float SpawnWeight => 1f;
		public virtual float Bouyancy => -0.1f;
		public virtual float Weight => 0.1f;

		private int floatingTimer = 0;

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (item.wet)
			{
				gravity = Bouyancy;
				if (item.velocity.Y < Bouyancy * 7)
					item.velocity.Y = Bouyancy * 7;

				Point tilePos = (item.position + new Vector2(0, 8 - item.velocity.Y)).ToTileCoordinates();
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
			Point tilePos = (item.position + new Vector2(0, 8 - item.velocity.Y)).ToTileCoordinates();
			if (Framing.GetTileSafely(tilePos.X, tilePos.Y - 1).liquid < 100 && item.wet && item.velocity.LengthSquared() < 0.01f)
			{
				Vector2 drawPos = item.position - Main.screenPosition - (Vector2.UnitY * (float)(Math.Sin((floatingTimer++ * 0.04f) - MathHelper.PiOver2) * 3f));
				spriteBatch.Draw(Main.itemTexture[item.type], drawPos, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
				return false;
			}
			return true;
		}
	}
}
