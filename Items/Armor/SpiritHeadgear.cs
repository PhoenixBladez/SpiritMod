using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SpiritHeadgear : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Headgear");
			Tooltip.SetDefault("Increases max life by 10 and increases melee damage by 12%");
		}

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.value = 40000;
            item.rare = 5;
            item.defense = 14;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("SpiritBodyArmor") && legs.type == mod.ItemType("SpiritLeggings");  
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Spirits grant you various buffs based on your health\nIncreases damage by 8% when below 400 health \nIncreases defense by 6 when below 200 Health \nIncreases life regen when below 100 Health \nIncreases invincibility frames when below 50 Health";

            if (player.statLife < 400)
            {
                player.meleeDamage += 0.08f;
                player.rangedDamage += 0.08f;
                player.thrownDamage += 0.08f;
                player.minionDamage += 0.08f;
                player.magicDamage += 0.08f;
            }
            if (player.statLife < 200)
            {
                player.statDefense += 6;
            }
            if (player.statLife < 100)
            {
                player.lifeRegen += 2;
            }
            if (player.statLife < 50)
            {
                player.longInvince = true;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 10;
            player.meleeDamage += 0.12f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritBar", 15);
            recipe.AddIngredient(null, "SoulShred", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}