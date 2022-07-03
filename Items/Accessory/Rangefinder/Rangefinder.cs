using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Rangefinder
{
	public class Rangefinder : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rangefinder");
			Tooltip.SetDefault("Ranged weapons are now equipped with a laser sight\nWorks while in the inventory");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 32;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
		}

		public override void UpdateInventory(Player player) => player.GetModPlayer<RangefinderPlayer>().active = true;
	}

	public class RangefinderPlayer : ModPlayer
	{
		public bool active = false;

		public override void ResetEffects() => active = false;
	}
}