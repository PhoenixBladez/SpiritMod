using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class HellsGaze : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Lash");
			Tooltip.SetDefault("Nearby enemies are engulfed in flames\n6% increased critical strike chance\nYou emit a fiery glow");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.buyPrice(gold: 8);
			item.expert = true;
			item.melee = true;
			item.accessory = true;

			item.knockBack = 9f;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().HellGaze = true;
			player.meleeCrit += 6;
			player.rangedCrit += 6;
			player.magicCrit += 6;
		}
	}
}
