using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class Atmos : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atmos Protector");
			Tooltip.SetDefault("Reduces damage taken by 4%\nMay nullify hostile projectiles before they hit the player");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Lime;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance += 0.04f;
			player.GetSpiritPlayer().atmos = true;
		}

	}
}
