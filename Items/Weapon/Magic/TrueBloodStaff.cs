using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
    public class TrueBloodStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Vessel Drainer");
			Tooltip.SetDefault("Shoots a clot that gathers power over time.");
		}


        public override void SetDefaults()
        {
            item.damage = 64;
            item.magic = true;
            item.mana = 20;
            item.width = 66;
            item.height = 68;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.crit = 15;
            item.value = 120000;
            item.rare = 8;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("TrueClot1");
            item.shootSpeed = 16f;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "BrokenStaff", 1);
            modRecipe.AddIngredient(null, "BloodStaff", 1);
            modRecipe.AddTile(134);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 0f, type, damage, knockBack, player.whoAmI);
            return false;
        }
    }
}