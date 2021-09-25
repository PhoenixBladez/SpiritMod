using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AccessoriesMisc.CrystalFlower
{
	//[AutoloadEquip(EquipType.Back)]
	public class CrystalFlowerItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Flower");
			Tooltip.SetDefault("Defeated enemies have a chance to drop damaging crystals on death");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().crystalFlower = true;
	}
}
