using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class SpiritStar : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Star");
            Tooltip.SetDefault("'The convergence of souls and the cosmos'\nRains down multiple starry bolts from the sky that inflict Star Fracture\nThese stars explode into multiple souls that inflict Soul Burn");

        }


        public override void SetDefaults()
        {
            item.damage = 112;
            item.useTime = 17;
            item.useAnimation = 17;
            item.melee = true;            
            item.width = 56;              
            item.height = 56;
            item.useStyle = 1;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 16, 0, 0);
            item.rare = 9;
            item.shootSpeed = 8;
            item.UseSound = SoundID.Item69;   
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = mod.ProjectileType("HarpyFeather");
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
                int dust1 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 226);

            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Vector2 mouse = Main.MouseWorld;
                    Projectile.NewProjectile(mouse.X + Main.rand.Next(-140, 140), player.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), mod.ProjectileType("SpiritStar"), damage, knockBack, player.whoAmI);
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Starblade", 1);
            recipe.AddIngredient(null, "StellarBlade", 1);
            recipe.AddIngredient(null, "SpiritSaber", 1);
            recipe.AddIngredient(null, "SpiritSword", 1);
            recipe.AddIngredient(null, "EtherealSword", 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 15);
            recipe.AddIngredient(ItemID.FragmentSolar, 4);
            recipe.AddIngredient(ItemID.FragmentVortex, 4);
            recipe.AddIngredient(ItemID.FragmentNebula, 4);
            recipe.AddIngredient(ItemID.FragmentStardust, 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}