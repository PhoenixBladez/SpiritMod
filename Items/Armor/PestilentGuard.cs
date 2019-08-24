using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PestilentGuard : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pestilent Guard");
			Tooltip.SetDefault("Increases bullet damage by 12%, and ranged critical strike chance by 5%");
		}
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
            item.rare = 4;
            item.defense = 7;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("PestilentPlatemail") && legs.type == mod.ItemType("PestilentLeggings");  
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Leave a deadly trail of cursed flames when you walk";
			((MyPlayer)player.GetModPlayer(mod, "MyPlayer")).flametrail = true;
        }

        public override void UpdateEquip(Player player)
        {
			player.bulletDamage += 0.12f;
            player.rangedCrit += 5;
        }
        
        		        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PutridPiece", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
