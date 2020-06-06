using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class BismiteSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Sword");
			Tooltip.SetDefault("Occasionally releases a Bismite Shard\nOccasionally causes foes to receive 'Festering Wounds,' which deal more damage to enemies under half health");
		}


        public override void SetDefaults()
        {
            item.damage = 11;            
            item.melee = true;            
            item.width = 32;              
            item.height = 32;             
            item.useTime = 16;           
            item.useAnimation = 16;     
            item.useStyle = ItemUseStyleID.SwingThrow;        
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item1;          
            item.shoot = mod.ProjectileType("BismiteSwordProjectile");
            item.shootSpeed = 7;               
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(5) == 0)
            {
                return true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"BismiteCrystal", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}