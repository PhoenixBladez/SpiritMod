using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class Starblade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starblade");
            Tooltip.SetDefault("'Harness the night sky'\nEvery fifth swing causes the blade to release multiple bright stars\nEach star explodes into homing star wisps");

        }


        int charger;
        public override void SetDefaults()
        {
            item.damage = 31;
            item.useTime = 22;
            item.useAnimation = 22;
            item.melee = true;            
            item.width = 50;              
            item.height = 50;
            item.useStyle = 1;        
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 4;
            item.shootSpeed = 8;
            item.UseSound = SoundID.Item1;   
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = mod.ProjectileType("GeodeStaveProjectile");
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 172);

            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger >= 5)
            {
                for (int I = 0; I < 4; I++)
                {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("Starshock2"), damage, knockBack, player.whoAmI, 0f, 0f);
                }
                charger = 0;
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "HarpyBlade", 1);
            recipe.AddIngredient(null, "TalonBlade", 1);
            recipe.AddIngredient(null, "Skyblade", 1);
            recipe.AddIngredient(null, "SteamParts", 4);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}