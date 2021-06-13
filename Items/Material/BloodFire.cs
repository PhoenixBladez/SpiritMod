using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class BloodFire : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dreamstride Essence");
			Tooltip.SetDefault("'The stuff of nightmares'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 6));
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = 100;
			item.rare = ItemRarityID.Green;

			item.maxStack = 999;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.92f, .14f, .24f);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}