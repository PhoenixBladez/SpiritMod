using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SeraphSet
{
	public class MoonStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Azure Gem");
			Tooltip.SetDefault("'Holds a far away power'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 3));
		}


		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = 1000;
			Item.rare = ItemRarityID.LightRed;
			Item.scale = .8f;
			Item.maxStack = 999;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			scale *= .6f;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(200, 200, 200, 100);
		}
	}
}