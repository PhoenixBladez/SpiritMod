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
			Tooltip.SetDefault("Normalizes gravity in space\nHold DOWN to increase gravity and vertical momentum\nNegates fall damage");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Blue;

			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.noFallDmg = true;
			if (player.gravity < 0.4f)
				player.gravity = 0.4f;
			if (player.controlDown)
			{
				player.portalPhysicsFlag = true;
				player._portalPhysicsTime = 2;
				if (player.velocity.Y > 0 && !player.gravControl) {
					Dust.NewDustPerfect(new Vector2(player.position.X + Main.rand.Next(player.width), player.position.Y + player.height - Main.rand.Next(7)), 206, Vector2.Zero);
				}
			}
		}
	}
}
