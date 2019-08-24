using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Weapon.Magic
{
    public class NightStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Horizon's Edge");
			Tooltip.SetDefault("Summons a portal from the Edge of the Horizon.");
		}


        public override void SetDefaults()
        {
            item.damage = 35;
            item.magic = true;
            item.mana = 14;
            item.width = 44;
            item.height = 48;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = 20000;
            item.rare = 4;
            item.crit += 9;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("CorruptPortal");
            item.shootSpeed = 13f;
        }

 		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "CorruptStaff", 1);
            modRecipe.AddIngredient(null, "JungleStaff", 1);
            modRecipe.AddIngredient(null, "DungeonStaff", 1);
            modRecipe.AddIngredient(null, "HellStaff", 1);
            modRecipe.AddTile(26);
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