using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
    public class StellarMask : ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Mask");
			Tooltip.SetDefault("10% increased ranged critical strike chance");
		}

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;
            item.defense = 12;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("StellarPlate") && legs.type == mod.ItemType("StellarLeggings");  
        }
		
        public override void UpdateArmorSet(Player player)
        {
			player.setBonus = "Increases the following stats while moving:\nIncreases ranged damage by 9%, ranged crit chance by 8%, and movement speed by 10%";
            if (player.velocity.X != 0)
			{
				player.rangedDamage += 0.09f;
				player.rangedCrit += 8;
				player.moveSpeed += 0.10f;
                int dust = Dust.NewDust(player.position, player.width, player.height, 133);
                Main.dust[dust].scale = 0.5f;
				Main.dust[dust].noGravity = true;
			}
            else if (player.velocity.Y != 0)
            {
                player.rangedDamage += 0.09f;
                player.rangedCrit += 8;
                player.moveSpeed += 0.10f;
                int dust = Dust.NewDust(player.position, player.width, player.height, 133);
                Main.dust[dust].scale = 0.5f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedCrit += 10;
        }
        
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StellarBar", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
