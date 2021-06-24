using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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
			item.width = 14;
			item.height = 14;
			item.maxStack = 1;
			item.knockBack = 1f;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Purple;
		}

		private float alpha;

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			alpha += 0.03f;
			float sineAdd = (float)Math.Sin(alpha);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			Vector4 colorMod = Color.Gold.ToVector4();
			SpiritMod.JemShaders.Parameters["distanceVar"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.JemShaders.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.JemShaders.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			SpiritMod.JemShaders.Parameters["rotation"].SetValue(alpha * 0.1f);
			SpiritMod.JemShaders.Parameters["opacity2"].SetValue(0.3f + (sineAdd / 10));
			SpiritMod.JemShaders.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), item.position - Main.screenPosition, null, Color.White, rotation, new Vector2(50, 50), (1.1f + (sineAdd / 9)) * scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			SpiritMod.JemShaders.Parameters["alpha"].SetValue(alpha * 2 % 6);
			SpiritMod.JemShaders.Parameters["coloralpha"].SetValue(alpha);
			SpiritMod.JemShaders.Parameters["shineSpeed"].SetValue(0.7f);
			SpiritMod.JemShaders.Parameters["map"].SetValue(mod.GetTexture("Textures/JemShaderMap"));
			SpiritMod.JemShaders.Parameters["lightColour"].SetValue(lightColor.ToVector3());
			SpiritMod.JemShaders.Parameters["shaderLerp"].SetValue(1f);
			SpiritMod.JemShaders.CurrentTechnique.Passes[1].Apply();
			spriteBatch.Draw(Main.itemTexture[item.type], item.position - Main.screenPosition, null, lightColor, rotation, new Vector2(Main.itemTexture[item.type].Width / 2, Main.itemTexture[item.type].Height / 2), scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			alpha += 0.03f;
			float sineAdd = (float)Math.Sin(alpha);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
			Vector4 colorMod = Color.Gold.ToVector4();
			SpiritMod.JemShaders.Parameters["distanceVar"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.JemShaders.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.JemShaders.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			SpiritMod.JemShaders.Parameters["rotation"].SetValue(alpha * 0.1f);
			SpiritMod.JemShaders.Parameters["opacity2"].SetValue(0.3f + (sineAdd / 10));
			SpiritMod.JemShaders.CurrentTechnique.Passes[0].Apply();
			Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), position + new Vector2(Main.itemTexture[item.type].Width / 2, Main.itemTexture[item.type].Height / 2), null, Color.White, 0f, new Vector2(50, 50), (1.1f + (sineAdd / 9)) * scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);

			SpiritMod.JemShaders.Parameters["alpha"].SetValue(alpha * 2 % 6);
			SpiritMod.JemShaders.Parameters["coloralpha"].SetValue(alpha);
			SpiritMod.JemShaders.Parameters["shineSpeed"].SetValue(0.7f);
			SpiritMod.JemShaders.Parameters["map"].SetValue(mod.GetTexture("Textures/JemShaderMap"));
			SpiritMod.JemShaders.Parameters["lightColour"].SetValue(drawColor.ToVector3());
			SpiritMod.JemShaders.Parameters["shaderLerp"].SetValue(1f);
			SpiritMod.JemShaders.CurrentTechnique.Passes[1].Apply();
			spriteBatch.Draw(Main.itemTexture[item.type], position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			return false;
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			if(line.Name == "ItemName") {
				Vector2 lineposition = new Vector2(line.OriginalX, line.OriginalY);
				Utils.DrawBorderString(Main.spriteBatch, line.text, lineposition, Color.LightGoldenrodYellow);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.UIScaleMatrix); //starting a new spritebatch here, since additive blend mode seems to be the only way to make the line transparent?
				for(int i = 0; i < 4; i++) {
					Vector2 drawpos = lineposition + new Vector2(0, 2 * (((float)Math.Sin(Main.GlobalTime * 4) / 2) + 0.5f)).RotatedBy(i * MathHelper.PiOver2);
					Utils.DrawBorderString(Main.spriteBatch, line.text, drawpos, Color.Goldenrod);
				}
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
				return false;
			}
			return base.PreDrawTooltipLine(line, ref yOffset);
		}
	}
}