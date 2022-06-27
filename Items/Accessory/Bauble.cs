using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class Bauble : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winter's Bauble");
			Tooltip.SetDefault("When under half health, damage taken is reduced by 10% and movement speed is increased by 5%\nWhen under half health, you are also surrounded by a shield that nullifies projectiles for 6 seconds\nTwo minute cooldown");
		}


		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().Bauble = true;
			if (player.statLife <= player.statLifeMax2 / 2) {
				player.endurance += .10f;
				player.moveSpeed += 0.05f;
			}
		}

	}
}
