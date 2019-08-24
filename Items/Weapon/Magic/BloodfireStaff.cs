using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
    public class BloodfireStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodfire Staff");
			Tooltip.SetDefault("Shoots a clump of blood that inflicts Blood Corruption");
		}


        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.value = Terraria.Item.sellPrice(0, 0, 18, 0);
            item.rare = 2;
            item.crit = 4;
            item.mana = 6;
            item.damage = 21;
            item.knockBack = 3;
            item.useStyle = 5;
            item.useTime = 27;
            item.useAnimation = 27;
            item.magic = true;
            item.noMelee = true;
            item.autoReuse = true;
            Item.staff[item.type] = true;
            item.shoot = mod.ProjectileType("BloodClump");
            item.shootSpeed = 8f;
            item.UseSound = SoundID.Item20;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}