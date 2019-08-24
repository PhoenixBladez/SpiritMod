using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using SpiritMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class ReflexBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatter Bow");
			Tooltip.SetDefault("Attacks occasionally pierce through enemies, lowering their defense");
		}



        public override void SetDefaults()
        {
            item.damage = 19; //This is the amount of damage the item does
            item.noMelee = true; //This makes sure the bow doesn't do melee damage
            item.ranged = true; //This causes your bow to do ranged damage
            item.width = 30; //Hitbox width
            item.height = 53; //Hitbox height
            item.value = Terraria.Item.sellPrice(0, 0, 6, 3);
            item.useTime = 25; //How long it takes to use the weapon. If this is shorter than the useAnimation it will fire twice in one click.
            item.useAnimation = 25;  //The animations time length
            item.useStyle = 5; //The style in which the item gets used. 5 for bows.
            item.shoot = 3; //Makes the bow shoot arrows
            item.useAmmo = AmmoID.Arrow;//Makes the bow consume arrows
            item.knockBack = 2; //The amount of knockback the item has
            item.rare = 2; //The item's name color
            item.UseSound = SoundID.Item5; //Sound that gets played on use
            item.autoReuse = true; //if the Bow autoreuses or not
            item.shootSpeed = 18f; //The arrows speed when shot
            item.crit = 4; //Crit chance
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>(mod).shotFromClatterBow = true;
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Carapace", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
