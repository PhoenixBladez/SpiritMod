using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class DemonfireSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flameberge Sword");
            Tooltip.SetDefault("Shoots out a fiery bolt\n~Donator Item~");

        }


        public override void SetDefaults()
        {
            item.damage = 49;
            item.useTime = 19;
            item.useAnimation = 19;
            item.melee = true;            
            item.width = 60;              
            item.height = 64;
            item.useStyle = 1;        
            item.knockBack = 4;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.rare = 7;
            item.shootSpeed = 12;
            item.UseSound = SoundID.Item1;   
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = mod.ProjectileType("PhoenixProjectile");
        }
        
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(4) == 0)
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                if (Main.rand.Next(5) == 0)
                {
                    int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
                }
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FieryEssence", 5);
			            recipe.AddIngredient(null, "CarvedRock", 10);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}