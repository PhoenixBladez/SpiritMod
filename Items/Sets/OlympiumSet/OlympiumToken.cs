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
		private int _frameCounter;
		private int _yFrame;
		private float _alpha;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Olympium Token");
			Tooltip.SetDefault("May be of interest to a collector...");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 300;
			item.rare = ItemRarityID.LightRed;
			item.createTile = ModContent.TileType<OlympiumToken_Tile>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			_frameCounter++;
			if (_frameCounter % 4 == 0)
				_yFrame++;

			_yFrame %= 4;
			if (Main.rand.Next(15) == 0)
			{
				int dust = Dust.NewDust(item.position, item.width, item.height, DustID.GoldCoin, 0, 0);
				Main.dust[dust].velocity = Vector2.Zero;
			}
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			_alpha += 0.05f;

			float sineAdd = (float)Math.Sin(_alpha);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			Vector4 colorMod = Color.Gold.ToVector4();
			SpiritMod.JemShaders.Parameters["distanceVar"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.JemShaders.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.JemShaders.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			SpiritMod.JemShaders.Parameters["rotation"].SetValue(_alpha * 0.1f);
			SpiritMod.JemShaders.Parameters["opacity2"].SetValue(0.3f + (sineAdd / 10));
			SpiritMod.JemShaders.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), item.Center - Main.screenPosition, null, Color.White, rotation, new Vector2(50, 50), (1.1f + (sineAdd / 9)) * scale * 0.5f, SpriteEffects.None, 0f);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			Texture2D tex = ModContent.GetTexture(Texture + "_World");
			Rectangle frame = new Rectangle(0, _yFrame * item.height, item.width, item.height);
			spriteBatch.Draw(tex, item.Center - Main.screenPosition, frame, lightColor, rotation, new Vector2(item.width, item.height) / 2, scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
	}
}
