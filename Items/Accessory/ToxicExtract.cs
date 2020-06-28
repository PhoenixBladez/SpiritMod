
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class ToxicExtract : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Extract");
			Tooltip.SetDefault("Magic attacks occasionally drench enemies in venom\n8% increased critical strike chance");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.defense = 3;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = 8;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 8;
			player.magicCrit += 8;
			player.rangedCrit += 8;
			player.GetSpiritPlayer().ToxicExtract = true;
		}

	}
}
