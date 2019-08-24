using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class MimeMask : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mime Mask");
            Tooltip.SetDefault("Increases summon damage by 3%\nIncreases maximum minions by 1");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 1;
            item.defense = 3;
        }
         public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.03f;
            player.maxMinions += 1;
        }
    }
}
