using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class ShadowBody : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Chestplate");
            Tooltip.SetDefault("Increases melee damage by 7%");

        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 22000;
            item.rare = 4;
            item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.07f;
        }
    }
}