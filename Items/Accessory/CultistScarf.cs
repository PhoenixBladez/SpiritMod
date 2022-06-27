using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Neck)]
	public class CultistScarf : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderweave Scarf");
			Tooltip.SetDefault("20% reduced mana usage when under half health\nIncreases maximum mana by 120 when above half health\n9% increased magic critical strike chance\nMagic attacks occasionally release bolts of powerful Ancient Magic that bounce off of walls");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(0, 90, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetCritChance(DamageClass.Magic) += 9;
			player.GetSpiritPlayer().cultistScarf = true;
			if (player.statLife < player.statLifeMax2 / 2)
				player.manaCost -= 0.20f;
			else 
				player.statManaMax2 += 120;
		}
	}
}
