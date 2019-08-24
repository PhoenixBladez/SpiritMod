using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class DesertStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dunewind Staff");
			Tooltip.SetDefault("Call upon the harsh desert winds!");
		}



        public override void SetDefaults()
        {
            item.damage = 60;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 54;
            item.height = 54;
            item.useTime = 80;
            item.mana = 44;
            item.useAnimation = 80;
            item.useStyle = 5;
            item.knockBack = 8;
            item.value = 80000;
            item.rare = 6;
            item.UseSound = SoundID.Item34;
            item.autoReuse = false;
            item.shootSpeed = 0;
            item.UseSound = SoundID.Item20;
            item.shoot = 656;
        }
                public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 10);
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 10);
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
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
