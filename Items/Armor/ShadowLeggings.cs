using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ShadowLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Leggings");
            Tooltip.SetDefault("Increases movement speed by 10% and melee damage by 6%");

        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 30000;
            item.rare = 4;
            item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.meleeDamage += 0.06f;
        }
    }
}