
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class MetalBand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Metal Band");
			Tooltip.SetDefault("Increases pickup range for ores\n'No you can't listen to this'");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0,0,20,0);
			item.rare = 1;
			item.defense = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().MetalBand = true;
		}

	}
}
