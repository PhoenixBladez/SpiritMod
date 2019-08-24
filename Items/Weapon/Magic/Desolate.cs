using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class Desolate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Desolation");
			Tooltip.SetDefault("Fires a sandstorm at your enemies");
		}


        public override void SetDefaults()
        {
            item.damage = 17;
            item.magic = true;
            item.mana = 6;
            item.width = 46;
            item.height = 46;
            item.useTime = 11;
            item.useAnimation = 11;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 290;
            item.rare = 3;
            item.UseSound = SoundID.Item34;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Sandstorm");
            item.shootSpeed = 20f;
            item.autoReuse = true;
        }
        

       public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DesertFossil, 10);
            recipe.AddIngredient(ItemID.Amber, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
