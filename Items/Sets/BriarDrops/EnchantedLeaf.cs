using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.BriarDrops
{
	public class EnchantedLeaf : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Leaf");
			Tooltip.SetDefault("'Blessed with the magic of druids'");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.maxStack = 999;
			Item.value = 500;
			Item.rare = ItemRarityID.Blue;
		}
		public override bool PreDrawInInventory(SpriteBatch sB, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			for (int i = 0; i < 1; i++)
			{
				int num7 = 16;
				float num8 = (float)(Math.Cos((double)Main.GlobalTimeWrappedHourly % 2.40000009536743 / 2.40000009536743 * MathHelper.TwoPi) / 5 + 0.5);
				SpriteEffects spriteEffects = SpriteEffects.None;
				Texture2D texture = TextureAssets.Item[Item.type].Value;
				Vector2 vector2_3 = new Vector2((float)(TextureAssets.Item[Item.type].Value.Width / 2), (TextureAssets.Item[Item.type].Value.Height / 1 / 2));
				var color2 = new Color(152, 250, 132, 150);
				Rectangle r = TextureAssets.Item[Item.type].Value.Frame(1, 1, 0, 0);
				for (int index2 = 0; index2 < num7; ++index2)
				{
					Color color3 = Item.GetAlpha(color2) * (0.85f - num8);
					Main.spriteBatch.Draw(texture, position + new Vector2(7, 8), new Microsoft.Xna.Framework.Rectangle?(r), color3, 0f, vector2_3, Item.scale * .58f + num8, spriteEffects, 0.0f);
				}
			}
			return true;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(new Vector2(Item.Center.X, Item.Center.Y), 81 * 0.001f, 194 * 0.001f, 58 * 0.001f);
			for (int i = 0; i < 1; i++)
			{
				int num7 = 16;
				float num8 = (float)(Math.Cos(Main.GlobalTimeWrappedHourly % 2.4 / 2.4 * MathHelper.TwoPi) / 5 + 0.5);
				SpriteEffects spriteEffects = SpriteEffects.None;
				Texture2D texture = TextureAssets.Item[Item.type].Value;
				var vector2_3 = new Vector2((TextureAssets.Item[Item.type].Value.Width / 2), (TextureAssets.Item[Item.type].Value.Height / 1 / 2));
				var color2 = new Color(152, 250, 132, 150);
				Rectangle r = TextureAssets.Item[Item.type].Value.Frame(1, 1, 0, 0);
				for (int index2 = 0; index2 < num7; ++index2)
				{
					Color color3 = Item.GetAlpha(color2) * (0.85f - num8);
					Vector2 position2 = Item.Center + ((index2 / num7 * MathHelper.TwoPi) + rotation).ToRotationVector2() * (4.0f * num8 + 2.0f) - Main.screenPosition - new Vector2(texture.Width, texture.Height) * Item.scale / 2f + vector2_3 * Item.scale;
					Main.spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position2, new Microsoft.Xna.Framework.Rectangle?(r), color3, rotation, vector2_3, Item.scale * 1.05f, spriteEffects, 0.0f);
				}
			}
			return true;
		}
	}
}
