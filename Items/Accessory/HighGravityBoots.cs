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
			Tooltip.SetDefault("Increases your gravity when falling and normalizes gravity in space\nBoosts horizontal momentum\nNegates fall damage");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.Blue;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.noFallDmg = true;
			if (player.gravity < 0.4f)
				player.gravity = 0.4f;
			player.portalPhysicsFlag = true;
			player._portalPhysicsTime = 2;
			if (player.velocity.Y > 0 && !player.gravControl) {
				Dust.NewDustPerfect(new Vector2(player.position.X + Main.rand.Next(player.width), player.position.Y + player.height - Main.rand.Next(7)), 206, Vector2.Zero);
			}
		}
	}
}
