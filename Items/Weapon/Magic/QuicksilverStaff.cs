using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class QuicksilverStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Wand");
			Tooltip.SetDefault("Shoots out a bolt of quicksilver that splits into smaller bouncing bolts of quicksilver energy");
		}


        int charger;
        private Vector2 newVect;
        public override void SetDefaults()
        {
            item.damage = 74;
            item.magic = true;
            item.mana = 9;
            item.width = 50;
            item.height = 60;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 2.5f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item21;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("QuicksilverBurst");
            item.shootSpeed = 8f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Material", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
