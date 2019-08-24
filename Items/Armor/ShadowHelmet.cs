using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ShadowHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Headgear");
			Tooltip.SetDefault("Increases melee speed by 8%\n Run with the Shadows...");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 40000;
            item.rare = 4;
            item.defense = 10;
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeSpeed += 0.08f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ShadowBody") && legs.type == mod.ItemType("ShadowLeggings");
        }
        public override void UpdateArmorSet(Player player)
        {

            player.setBonus = "Become the Shadow upon reaching half health, increasing movement speed by 25% and melee damage by 10%";

            if (player.statLife < player.statLifeMax2 / 2)
            {
                player.moveSpeed += 0.25f;
                player.meleeDamage += 0.1f;
                int dust = Dust.NewDust(player.position, player.width, player.height, 21);
            }
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
    }
}
