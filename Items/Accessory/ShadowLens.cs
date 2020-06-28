using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class ShadowLens : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Animus Lens");
			Tooltip.SetDefault("Summons guardians of spirit and shadow to protect you\nGuardians gain increased damage and attack speed when under half health\n7% increased critical strike chance");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = Item.buyPrice(0, 0, 11, 0);
			item.rare = ItemRarityID.Pink;
			item.expert = true;
			item.defense = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 7;
			player.magicCrit += 7;
			player.rangedCrit += 7;
			player.GetSpiritPlayer().animusLens = true;
		}

	}
}
