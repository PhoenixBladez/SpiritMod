using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class RogueHood : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rogue Hood");
			Tooltip.SetDefault("Increases movement speed by 4%");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = Terraria.Item.buyPrice(0, 0, 50, 0);
            item.rare = 1;
            item.defense = 1;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.04f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<RoguePlate>() && legs.type == ModContent.ItemType<RoguePants>();  
        }
        public override void UpdateArmorSet(Player player)
        { 
            player.setBonus = "Getting hit grants the player four seconds of invisibility\nWhile invisible this way, damage dealt is increased by 100%\n25 second cooldown";
            player.GetSpiritPlayer().rogueSet = true;

            if (player.HasBuff(ModContent.BuffType<RogueCooldown>()))
            {
                if (player.HasBuff(BuffID.Invisibility))
                {
                    player.rangedDamage += 1f;
                    player.meleeDamage += 1f;
                    player.magicDamage += 1f;
                    player.minionDamage += 1f;
                }
            }
        }
    }
}