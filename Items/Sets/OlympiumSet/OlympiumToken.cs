using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.OlympiumSet
{
	public class OlympiumToken : ModItem
	{
		private int _frameCounter;
		private int _yFrame;
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
			Texture2D tex = ModContent.GetTexture(Texture + "_World");
			Rectangle frame = new Rectangle(0, _yFrame * item.height, item.width, item.height);
			spriteBatch.Draw(tex, item.Center - Main.screenPosition, frame, lightColor, rotation, new Vector2(item.width, item.height) / 2, scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
