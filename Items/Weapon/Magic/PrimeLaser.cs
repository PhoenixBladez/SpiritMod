using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class PrimeLaser : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PrimeLaser");
			Tooltip.SetDefault("Shoots beams of Mechanical Energy!");
		}


        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 30;
            item.value = Item.buyPrice(0, 8, 0, 0);
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
            item.crit += 8;
            item.damage = 46;
            item.useStyle = 5;
            item.knockBack = 2;
            item.useTime = 9;
            item.useAnimation = 10;
            item.mana = 5;
            item.reuseDelay = 5;
            item.magic = true;
            item.channel = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.shoot = 389;
            item.shootSpeed = 16f;
            
        }
            public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PrintPrime", 1);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddIngredient(ItemID.SoulofFright, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

