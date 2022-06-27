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
			Item.width = 30;
			Item.height = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().crystalFlower = true;
	}
}
