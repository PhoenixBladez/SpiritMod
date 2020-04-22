using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class ShurikenLauncher : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thrower's Glove");
			Tooltip.SetDefault("Increases throwing damage and critical strike chance by 5%\nAfter every 10 successful throwing strikes, your next attack will do more damage and fly faster");
		}


        public override void SetDefaults()
        {
            item.width = 38;     
            item.height = 38;   
            item.value = Item.buyPrice(0, 8, 0, 0);
            item.rare = 2;

            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.thrownDamage += 0.05f;
            player.thrownCrit += 5;
            player.GetSpiritPlayer().throwerGlove = true;
        }
    }
}
