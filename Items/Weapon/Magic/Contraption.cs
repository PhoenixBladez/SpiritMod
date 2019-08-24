using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
    public class Contraption : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crazed Contraption");
			Tooltip.SetDefault("'What does it do? No one knows!'");
		}



        public override void SetDefaults()
        {
            item.damage = 120;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 28;
            item.height = 28;
            item.useTime = 9;
            item.mana = 10;
            item.useAnimation = 9;
            item.useStyle = 5;
            item.knockBack = 10;
            item.value = 200000;
            item.rare = 9;
            item.UseSound = SoundID.Item92;
            item.autoReuse = true;
            item.shootSpeed = 15;
            item.UseSound = SoundID.Item20;
            item.shoot = mod.ProjectileType("Starshock1");
        }
             public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int p = Main.rand.Next(1, 714);
            {
                int pl = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, p, damage, knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[pl].friendly = true;
                Main.projectile[pl].hostile = false;
            }
            return false;
        }
        public override void AddRecipes()
        {

            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "SteamParts", 10);
            modRecipe.AddIngredient(null, "TechDrive", 10);
            modRecipe.AddIngredient(null, "PrintPrime", 1);
            modRecipe.AddIngredient(null, "PrintProbe", 1);
            modRecipe.AddIngredient(null, "BlueprintTwins", 1);
            modRecipe.AddIngredient(null, "SpiritBar", 10);
            modRecipe.AddIngredient(null, "StellarBar", 10);
            modRecipe.AddIngredient(ItemID.FragmentVortex, 2);
            modRecipe.AddIngredient(ItemID.FragmentNebula, 2);
            modRecipe.AddIngredient(ItemID.FragmentStardust, 2);
            modRecipe.AddIngredient(ItemID.FragmentSolar, 2);
            modRecipe.AddIngredient(ItemID.Cog, 25);
            modRecipe.AddIngredient(ItemID.Ectoplasm, 6);
            modRecipe.AddIngredient(ItemID.LihzahrdPowerCell, 2);
            modRecipe.AddTile(134);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
