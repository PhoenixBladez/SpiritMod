using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PestilentVisor : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pestilent Visor");
			Tooltip.SetDefault("Increases arrow damage by 10% and ranged critical strike chance by 6%");
		}

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Terraria.Item.sellPrice(0, 0, 91, 0);
            item.rare = 4;

            item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("PestilentPlatemail") && legs.type == mod.ItemType("PestilentLeggings");  
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Every 4th hit with a pestilent or ranged weapon causes an explosion of cursed flames";
			player.GetModPlayer<MyPlayer>(mod).putridSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.arrowDamage += 0.1f;
            player.rangedCrit += 6;
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PutridPiece", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
