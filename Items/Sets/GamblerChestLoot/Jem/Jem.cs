using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GamblerChestLoot.Jem
{
	public class Jem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jem");
			Tooltip.SetDefault("Holds untold power");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 14;
			Item.maxStack = 1;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.rare = ItemRarityID.Purple;
		}


		private float alpha;

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			alpha += 0.03f;

			float sineAdd = (float)Math.Sin(alpha);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			Vector4 colorMod = Color.Gold.ToVector4();
			SpiritMod.JemShaders.Parameters["distanceVar"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.JemShaders.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.JemShaders.Parameters["noise"].SetValue(Mod.GetTexture("Textures/noise"));
			SpiritMod.JemShaders.Parameters["rotation"].SetValue(alpha * 0.1f);
			SpiritMod.JemShaders.Parameters["opacity2"].SetValue(0.3f + (sineAdd / 10));
			SpiritMod.JemShaders.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(Mod.GetTexture("Effects/Masks/Extra_49"), Item.position - Main.screenPosition, null, Color.White, rotation, new Vector2(50, 50), (1.1f + (sineAdd / 9)) * scale, SpriteEffects.None, 0f);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			SpiritMod.JemShaders.Parameters["alpha"].SetValue(alpha * 2 % 6);
			SpiritMod.JemShaders.Parameters["coloralpha"].SetValue(alpha);
			SpiritMod.JemShaders.Parameters["shineSpeed"].SetValue(0.7f);
			SpiritMod.JemShaders.Parameters["map"].SetValue(Mod.GetTexture("Textures/JemShaderMap"));
			SpiritMod.JemShaders.Parameters["lightColour"].SetValue(lightColor.ToVector3());
			SpiritMod.JemShaders.Parameters["shaderLerp"].SetValue(1f);
			SpiritMod.JemShaders.CurrentTechnique.Passes[1].Apply();

			spriteBatch.Draw(TextureAssets.Item[Item.type].Value, Item.position - Main.screenPosition, null, lightColor, rotation, new Vector2(TextureAssets.Item[Item.type].Value.Width / 2, TextureAssets.Item[Item.type].Value.Height / 2), scale, SpriteEffects.None, 0f);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			alpha += 0.03f;
			float sineAdd = (float)Math.Sin(alpha);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);

			Vector4 colorMod = Color.Gold.ToVector4();
			SpiritMod.JemShaders.Parameters["distanceVar"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.JemShaders.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.JemShaders.Parameters["noise"].SetValue(Mod.GetTexture("Textures/noise"));
			SpiritMod.JemShaders.Parameters["rotation"].SetValue(alpha * 0.1f);
			SpiritMod.JemShaders.Parameters["opacity2"].SetValue(0.3f + (sineAdd / 10));
			SpiritMod.JemShaders.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(Mod.GetTexture("Effects/Masks/Extra_49"), position + new Vector2(TextureAssets.Item[Item.type].Value.Width / 2, TextureAssets.Item[Item.type].Value.Height / 2), null, Color.White, 0f, new Vector2(50, 50), (1.1f + (sineAdd / 9)) * scale, SpriteEffects.None, 0f);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);

			SpiritMod.JemShaders.Parameters["alpha"].SetValue(alpha * 2 % 6);
			SpiritMod.JemShaders.Parameters["coloralpha"].SetValue(alpha);
			SpiritMod.JemShaders.Parameters["shineSpeed"].SetValue(0.7f);
			SpiritMod.JemShaders.Parameters["map"].SetValue(Mod.GetTexture("Textures/JemShaderMap"));
			SpiritMod.JemShaders.Parameters["lightColour"].SetValue(drawColor.ToVector3());
			SpiritMod.JemShaders.Parameters["shaderLerp"].SetValue(1f);
			SpiritMod.JemShaders.CurrentTechnique.Passes[1].Apply();

			spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			return false;
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			if (line.Name == "ItemName")
			{
				Vector2 lineposition = new Vector2(line.OriginalX, line.OriginalY);
				Utils.DrawBorderString(Main.spriteBatch, line.Text, lineposition, Color.LightGoldenrodYellow);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.UIScaleMatrix); //starting a new spritebatch here, since additive blend mode seems to be the only way to make the line transparent?

				for (int i = 0; i < 4; i++)
				{
					Vector2 drawpos = lineposition + new Vector2(0, 2 * (((float)Math.Sin(Main.GlobalTimeWrappedHourly * 4) / 2) + 0.5f)).RotatedBy(i * MathHelper.PiOver2);
					Utils.DrawBorderString(Main.spriteBatch, line.Text, drawpos, Color.Goldenrod);
				}

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
				return false;
			}
			return base.PreDrawTooltipLine(line, ref yOffset);
		}
	}
}