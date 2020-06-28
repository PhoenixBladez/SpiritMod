using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class GravityModulator : ModItem
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Modulator");
			Tooltip.SetDefault("Reduces the player's gravity significantly");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 32;
			item.accessory = true;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Green;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.gravity = .05f;
			DoDust(player.position, player.width, player.height);
		}
		private void DoDust(Vector2 position, int width, int height)
		{
			if(Main.rand.Next(4) == 0) {
				int d = Dust.NewDust(position, width, height, 172);
				Main.dust[d].scale *= .9f;
				Main.dust[d].velocity *= -.9f;
				Main.dust[d].noGravity = true;
			}
		}
	}
}
