using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Shoes)]
	public class HighGravityBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("High-Grav Boots");
			Tooltip.SetDefault("Increases your gravity when falling");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.value = Item.buyPrice(0, 0, 4, 0);
			item.rare = ItemRarityID.Blue;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(player.velocity.Y > 0 && !player.gravControl) {
				player.velocity.Y = 7f;
				Dust.NewDustPerfect(new Vector2(player.position.X + Main.rand.Next(player.width), player.position.Y + player.height - Main.rand.Next(7)), 206, Vector2.Zero);
			}
		}
	}
}
