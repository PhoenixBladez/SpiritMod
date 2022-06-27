using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.SlagSet
{
	public class CarvedRock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slagstone");
			Tooltip.SetDefault("'A seething piece of hardened magma'");
		}


		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = 800;
			Item.rare = ItemRarityID.Orange;

			Item.maxStack = 999;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.4f, .12f, .036f);
		}
	}
}
