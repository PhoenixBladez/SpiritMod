using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class ChloroStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chlorophyte Staff");
			Tooltip.SetDefault("Summons a cloud of natural essences");
		}



        public override void SetDefaults()
        {
            item.damage = 52;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 58;
            item.height = 58;
            item.useTime = 34;
            item.mana = 12;
            item.useAnimation = 34;
            item.useStyle = 5;
            item.knockBack = 8;
            item.value = 90000;
            item.rare = 7;
            item.UseSound = SoundID.Item34;
            item.autoReuse = true;
            item.shootSpeed = 4;
            item.UseSound = SoundID.Item20;
            item.shoot = mod.ProjectileType("GrassPortal");
        }
                public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 0f, type, damage, knockBack, player.whoAmI);
            return false;
        }
    }
}
