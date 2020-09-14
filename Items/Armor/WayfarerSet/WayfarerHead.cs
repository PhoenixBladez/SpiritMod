using SpiritMod.Items.Material;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WayfarerSet
{
	[AutoloadEquip(EquipType.Head)]
	public class WayfarerHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wayfarer's Hat");
			Tooltip.SetDefault("Immunity to darkness");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
			item.rare = 1;
			item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
            player.buffImmune[BuffID.Darkness] = true;
        }

		public override void UpdateArmorSet(Player player)
		{
			player.GetSpiritPlayer().wayfarerSet = true;
			player.setBonus = "Killing enemies grants a stacking damage buff\nBreaking pots grants a stacking movement speed buff\nMining ore grants a stacking mining speed buff\nAll buffs stack up to 4 times";
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
			=> body.type == ModContent.ItemType<WayfarerBody>()
			&& legs.type == ModContent.ItemType<WayfarerLegs>();

	}
}
