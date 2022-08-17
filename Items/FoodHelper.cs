using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items;

internal class FoodHelper
{
	public static bool PreDrawInInventory(ModItem item, SpriteBatch spriteBatch, Vector2 position, Color drawColor, float scale, Vector2? offset = null)
	{
		offset = offset ?? new Vector2(0, item.Item.width * (10 / 30f));
		Texture2D tex = ModContent.Request<Texture2D>(item.Texture).Value;
		spriteBatch.Draw(tex, position.ToPoint().ToVector2() + offset.Value, new Rectangle(0, 0, item.Item.width, item.Item.height), drawColor, 0f, item.Item.Size / 3f, scale * 3, SpriteEffects.None, 0f);
		return false;
	}

	public static bool PreDrawInWorld(ModItem item, SpriteBatch spriteBatch, Color lightColor, ref float rotation, ref float scale)
	{
		Texture2D tex = ModContent.Request<Texture2D>(item.Texture).Value;
		spriteBatch.Draw(tex, item.Item.Center - Main.screenPosition, new Rectangle(0, 0, item.Item.width, item.Item.height), lightColor, rotation, item.Item.Size / 2f, scale, SpriteEffects.None, 0f);
		return false;
	}
}
