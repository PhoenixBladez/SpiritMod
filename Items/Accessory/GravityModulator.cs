using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class GravityModulator : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Modulator");
			Tooltip.SetDefault("Reduces the player's gravity significantly");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.accessory = true;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.gravity = .05f;
			DoDust(player.position, player.width, player.height);
		}
		private void DoDust(Vector2 position, int width, int height)
		{
			if (Main.rand.Next(4) == 0) {
				int d = Dust.NewDust(position, width, height, DustID.DungeonWater);
				Main.dust[d].scale *= .9f;
				Main.dust[d].velocity *= -.9f;
				Main.dust[d].noGravity = true;
			}
		}
	}
}
