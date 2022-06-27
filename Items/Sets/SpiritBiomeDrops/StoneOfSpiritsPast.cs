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
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
			Item.defense = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().SoulStone = true;
		}
	}
}
