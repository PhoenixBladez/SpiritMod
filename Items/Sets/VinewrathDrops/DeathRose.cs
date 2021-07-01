using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SpiritMod.Items.Sets.VinewrathDrops
{
	public class DeathRose : ModItem
	{
		public override void SetStaticDefaults()
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.DOWN" : "Key.UP");
			DisplayName.SetDefault("Briar Blossom");
			Tooltip.SetDefault($"Double tap {tapDir} to ensnare an enemy at the cursor position\n4 second cooldown");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 36;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().deathRose = true;
		}
	}
}
