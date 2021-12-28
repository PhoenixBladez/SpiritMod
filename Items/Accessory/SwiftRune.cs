using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class SwiftRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swiftness Rune");
			Tooltip.SetDefault("Massively increases unassisted aerial agility");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.buyPrice(gold: 5);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.velocity.Y != 0 && player.wings <= 0 && !player.mount.Active) {
				player.runAcceleration *= 2f;
				player.maxRunSpeed *= 1.5f;
			}
		}
	}
}
