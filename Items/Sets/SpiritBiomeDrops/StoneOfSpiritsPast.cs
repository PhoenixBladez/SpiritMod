using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritBiomeDrops
{
	public class StoneOfSpiritsPast : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Of Spirits Past");
			Tooltip.SetDefault("Creates orbiting souls to hurt nearby enemies");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
			item.defense = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().SoulStone = true;
		}
	}
}
