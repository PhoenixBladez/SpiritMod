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
			Tooltip.SetDefault("Increases ranged damage by 10% and ranged critical strike chance by 5%\nIncreases your maximum number of minions by 1");
		}

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;
            item.defense = 7;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("StellarPlate") && legs.type == mod.ItemType("StellarLeggings");  
        }
		
        public override void UpdateArmorSet(Player player)
        {
			player.setBonus = "Ranged critical strikes empower your minions with increased attack and knockback\nKilling enemies with minions empowers your movement speed";
            player.GetSpiritPlayer().stellarSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.1f;
            player.rangedCrit += 5;
            player.maxMinions += 1;
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
