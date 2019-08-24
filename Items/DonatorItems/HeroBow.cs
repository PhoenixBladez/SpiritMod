using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using SpiritMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    public class HeroBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hero's Bow");
			Tooltip.SetDefault("Right-click to shoot either fiery, icy, and light arrows with different effects \n -Fiery arrows can inflict multiple different burns on foes \n -Icy Arrows can freeze an enemy in place and frostburn hit foes \n -Light Arrows have a 2% chance to instantly kill any non-boss enemy\n -Regular Arrows have powerful damage and knockback \n ~Donator Item~");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 65;
            item.noMelee = true;
            item.ranged = true;
            item.width = 22;
            item.height = 46;
            item.useTime = 19;
            item.useAnimation = 19;
            item.useStyle = 5;
            item.shoot = 1;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 7;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.useTurn = false;
            item.shootSpeed = 14.2f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {

                if (Main.rand.Next(3) == 1)
                {
                    int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                     Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>(mod).HeroBow3 = true;

                    return false;
                }

                else if (Main.rand.Next(2) == 1)
                {
                    int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                     Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>(mod).HeroBow2 = true;

                    return false;
                }
                else
                {
                    int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                    Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>(mod).HeroBow1 = true;

                    return false;
                }
                return false;
            }
            else
            {

                int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ectoplasm, 8);
            recipe.AddIngredient(null, "AncientBark", 10);
            recipe.AddIngredient(null, "OldLeather", 10);
            recipe.AddIngredient(null, "SpiritBar", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}