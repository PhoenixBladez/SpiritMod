using Terraria;
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
			item.width = item.height = 16;
			item.maxStack = 999;
			item.value = 500;
			item.rare = ItemRarityID.Blue;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) 
		{
			Lighting.AddLight(new Vector2(item.Center.X, item.Center.Y), 81*0.001f, 194*0.001f, 58*0.001f);
			for (int i = 0; i<1; i++)
			{
				int num7 = 16;
				float num9 = 6f;
				float num8 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 5 +0.5);
				float amount1 = 0.5f;
				float num10 = 0.0f;
				float addY = 0f;
				float addHeight = 0f;
				SpriteEffects spriteEffects = SpriteEffects.None;
				Texture2D texture = Main.itemTexture[item.type];
				Vector2 vector2_3 = new Vector2((float) (Main.itemTexture[item.type].Width / 2), (float) (Main.itemTexture[item.type].Height / 1 / 2));
				Vector2 position1 = item.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * item.scale / 2f + vector2_3 * item.scale + new Vector2(0.0f, addY + addHeight + 0);
				Microsoft.Xna.Framework.Color color2 = new Color(152, 250, 132, 150);
				Microsoft.Xna.Framework.Rectangle r = Main.itemTexture[item.type].Frame(1, 1, 0, 0);
				for (int index2 = 0; index2 < num7; ++index2)
				{
					Microsoft.Xna.Framework.Color newColor2 = color2;
					Microsoft.Xna.Framework.Color color3 = item.GetAlpha(newColor2) * (0.85f - num8);
					Vector2 position2 = new Vector2 (item.Center.X,item.Center.Y) + ((float) ((double) index2 / (double) num7 * 6.28318548202515) + rotation + num10).ToRotationVector2() * (float) (4.0 * (double) num8 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / 1)) * item.scale / 2f + vector2_3 * item.scale + new Vector2(0.0f, addY + addHeight + 0);
					Main.spriteBatch.Draw(Main.itemTexture[item.type], position2, new Microsoft.Xna.Framework.Rectangle?(r), color3, rotation, vector2_3, item.scale*1.05f, spriteEffects, 0.0f);
				}
			}
			return true;
		}
	}
}
