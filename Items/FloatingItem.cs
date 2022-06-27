using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
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
			if (Item.wet)
			{
				gravity = Bouyancy;
				if (Item.velocity.Y < Bouyancy * 7)
					Item.velocity.Y = Bouyancy * 7;

				Point tilePos = (Item.position + new Vector2(0, 8 - Item.velocity.Y)).ToTileCoordinates();
				if (Framing.GetTileSafely(tilePos.X, tilePos.Y - 1).LiquidAmount < 100)
				{
					gravity = 0;
					Item.velocity.Y *= 0.9f;
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
			Point tilePos = (Item.position + new Vector2(0, 8 - Item.velocity.Y)).ToTileCoordinates();
			if (Framing.GetTileSafely(tilePos.X, tilePos.Y - 1).LiquidAmount < 100 && Item.wet && Item.velocity.LengthSquared() < 0.01f)
			{
				Vector2 drawPos = Item.position - Main.screenPosition - (Vector2.UnitY * (float)(Math.Sin((floatingTimer++ * 0.04f) - MathHelper.PiOver2) * 3f));
				spriteBatch.Draw(TextureAssets.Item[Item.type].Value, drawPos, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
				return false;
			}
			return true;
		}
	}
}
