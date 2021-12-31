using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.OlympiumSet
{
	public class OlympiumToken : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Olympium Token");
			Tooltip.SetDefault("May be of interest to a collector...");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = 0;
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 999;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D glowTex = ModContent.GetTexture(Texture + "_glow");
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");

			float timer = (float)(Math.Sin(Main.GlobalTime * 3) / 2) + 0.5f;
			Color color = new Color(255, 238, 125, 0) * 0.5f;

			Vector2 itemCenter = new Vector2(item.position.X - Main.screenPosition.X + item.width / 2, item.position.Y - Main.screenPosition.Y + item.height - (Main.itemTexture[item.type].Height / 2) + 2f);

			//Draw bloom beneath the item texture
			spriteBatch.Draw(bloom, itemCenter, null, color * 0.5f, rotation, bloom.Size() / 2, 0.4f, SpriteEffects.None, 0);

			//Make less transparent after drawing bloom
			color.A = 100;

			//Draw a pulse glowmask effect and glowmask
			for (int i = 0; i < 5; i++)
			{
				Vector2 offset = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 5) * 7 * timer;
				float opacity = 1 - timer;
				opacity *= 0.5f;

				spriteBatch.Draw(glowTex, itemCenter + offset, null, color * opacity, rotation, glowTex.Size() / 2, scale * 1.15f, SpriteEffects.None, 0);
			}

			spriteBatch.Draw(glowTex, itemCenter, null, color, rotation, glowTex.Size() / 2, scale * 1.15f, SpriteEffects.None, 0);
			return true;
		}
	}
}
