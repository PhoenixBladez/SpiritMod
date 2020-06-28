
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class Crystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Distorter");
			Tooltip.SetDefault("Ranged weapons have a chance to fire multiple projectiles\nThe duplicate projectiles usually lack special effects");

		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = 5;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().crystal = true;
		}
	}
}
