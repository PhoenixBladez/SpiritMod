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
			Item.width = 16;
			Item.height = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 300;
			Item.rare = ItemRarityID.LightRed;
			Item.createTile = ModContent.TileType<OlympiumToken_Tile>();
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			_frameCounter++;
			if (_frameCounter % 4 == 0)
				_yFrame++;

			_yFrame %= 4;
			if (Main.rand.Next(15) == 0)
			{
				int dust = Dust.NewDust(Item.position, Item.width, Item.height, DustID.GoldCoin, 0, 0);
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
			SpiritMod.JemShaders.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("Textures/noise").Value);
			SpiritMod.JemShaders.Parameters["rotation"].SetValue(_alpha * 0.1f);
			SpiritMod.JemShaders.Parameters["opacity2"].SetValue(0.3f + (sineAdd / 10));
			SpiritMod.JemShaders.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value, Item.Center - Main.screenPosition, null, Color.White, rotation, new Vector2(50, 50), (1.1f + (sineAdd / 9)) * scale * 0.5f, SpriteEffects.None, 0f);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			Texture2D tex = ModContent.Request<Texture2D>(Texture + "_World");
			Rectangle frame = new Rectangle(0, _yFrame * Item.height, Item.width, Item.height);
			spriteBatch.Draw(tex, Item.Center - Main.screenPosition, frame, lightColor, rotation, new Vector2(Item.width, Item.height) / 2, scale, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
	}
}
