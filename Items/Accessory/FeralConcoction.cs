
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class FeralConcoction : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feral Concotion");
			Tooltip.SetDefault("Immunity to Feral Bite");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Green;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[BuffID.Rabies] = true;
		}
	}
}
