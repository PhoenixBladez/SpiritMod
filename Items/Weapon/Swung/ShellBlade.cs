using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class ShellBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Blade");
			Tooltip.SetDefault("Shoots a tidal shard which waxes and wanes in velocity\n Occasionally inflicts Tidal Ebb, which lowers enemy defense and life");
		}


        public override void SetDefaults()
        {
            item.damage = 26;            
            item.melee = true;            
            item.width = 40;              
            item.height = 44;
            item.useTime = 36;
            item.useAnimation = 36;     
            item.useStyle = 1;
            item.shoot = mod.ProjectileType("TidalShard");
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 0, 30, 0);
            item.rare = 3;
            item.shootSpeed = 6f;
            item.UseSound = SoundID.Item1;        
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 4);
            recipe.AddIngredient(null, "PearlFragment", 12);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}