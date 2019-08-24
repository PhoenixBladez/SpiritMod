using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class StellarBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Blade");
			Tooltip.SetDefault("Inflicts Star Fracture");
		}


        public override void SetDefaults()
        {
            item.damage = 46;            
            item.melee = true;            
            item.width = 48;              
            item.height = 48;             
            item.useTime = 25;           
            item.useAnimation = 25;     
            item.useStyle = 1;        
            item.knockBack = 4;
            item.shoot = mod.ProjectileType("StarfallProjectile");
            item.shootSpeed = 16f;   
            item.value = 1960;        
            item.rare = 5;
            item.UseSound = SoundID.Item1;     
            item.autoReuse = true;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.value = Item.sellPrice(0, 1, 0, 0);
            item.useTurn = true;
            item.crit = 4;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(mod.BuffType("StarFracture"), 300);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 133);
            Main.dust[dust].noGravity = true;
			}
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"StellarBar", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}