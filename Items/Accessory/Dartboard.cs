using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class Dartboard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dartboard");
			Tooltip.SetDefault("13% reduced damage\n15% increased critical strike chance\n'Right on the mark'");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.allDamage -= 0.13f;
			player.meleeCrit += 15;
			player.magicCrit += 15;
			player.rangedCrit += 15;

		}
	}
}
