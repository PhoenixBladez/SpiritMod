using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FHelmet : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Helmet");
			Tooltip.SetDefault("5% Increased magic damage and critical strike chance\nReduces mana cost by 5%\nIt's natural, yet seems to be from somwhere else...");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 22;
            item.value = Terraria.Item.sellPrice(0, 0, 12, 0);
            item.rare = 2;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.magicCrit += 4;
            player.magicDamage += 0.05f;
            player.manaCost -= 0.05f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("FPlate") && legs.type == mod.ItemType("FLegs");  
        }
        public override void UpdateArmorSet(Player player)
        {
            timer++;

            if (timer == 20)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, 44);
                timer = 0;
            }

            player.setBonus = "Florrrann Sssstab- 5% increased damage and 5% increased movement speed";
            player.meleeDamage += 0.05f;
            player.thrownDamage += 0.05f;
            player.rangedDamage += 0.05f;
            player.magicDamage += 0.05f;
            player.minionDamage += 0.05f;

            player.maxRunSpeed += 0.05f;

        }
        public override void AddRecipes()  
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FloranBar", 12);
            recipe.AddTile(TileID.Anvils);   
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
