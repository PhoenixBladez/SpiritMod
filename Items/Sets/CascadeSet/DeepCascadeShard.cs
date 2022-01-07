using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;

namespace SpiritMod.Items.Sets.CascadeSet
{
	public class DeepCascadeShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deep Cascade Shard");
			Tooltip.SetDefault("'Where magma meets seawater'");
		}


		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 18;
			item.value = 400;
			item.rare = ItemRarityID.Blue;

			item.maxStack = 999;
		}

		private bool chosenStyle = false;
		private int _yFrame;
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (!chosenStyle)
			{
				_yFrame = Main.rand.Next(0, 3);
				chosenStyle = true;
			}
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(new Vector2(item.Center.X, item.Center.Y), 207 * 0.001f, 12 * 0.001f, 12 * 0.001f);

			Texture2D tex = ModContent.GetTexture(Texture + "_World");
			Rectangle frame = new Rectangle(0, _yFrame * 18, 10, 18);
			int num7 = 16;
			float num8 = (float)(Math.Cos(Main.GlobalTime % 2.4 / 2.4 * MathHelper.TwoPi) / 5 + 0.5);
			var color2 = new Color(252, 57, 3, 100);
			spriteBatch.Draw(tex, item.Center - Main.screenPosition, frame, lightColor, rotation, new Vector2(item.width, item.height) / 2, scale, SpriteEffects.None, 0f);

			for (int index2 = 0; index2 < num7; ++index2)
			{
				Color color3 = item.GetAlpha(color2) * (0.85f - num8);
				Vector2 position2 = item.Center + ((index2 / num7 * MathHelper.TwoPi) + rotation).ToRotationVector2() * (4.0f * num8 + 2.0f) - Main.screenPosition - new Vector2(tex.Width, tex.Height) * item.scale / 2f + new Vector2(item.width, item.height) / 2 * item.scale;
				Main.spriteBatch.Draw(tex, position2 + new Vector2(-4, 20), frame, color3, rotation, new Vector2(item.width, item.height) / 2, item.scale * 1.05f, SpriteEffects.None, 0.0f);
			}
			return false;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D tex2 = ModContent.GetTexture(Texture + "_World_Glow");
			Rectangle frame = new Rectangle(0, _yFrame * 18, 10, 18);

			spriteBatch.Draw(tex2, item.Center - Main.screenPosition, frame, Color.White, rotation, new Vector2(item.width, item.height) / 2, scale, SpriteEffects.None, 0f);
		}
	}
}
