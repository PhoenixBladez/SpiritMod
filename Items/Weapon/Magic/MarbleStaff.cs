using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class MarbleStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Staff");
			Tooltip.SetDefault("Shoots out a bolt of golden energy that hits enemies twice \n Right click to summon a portal of energy at the cursor position");
		}


		public override void SetDefaults()
		{
			item.damage = 21;
			item.magic = true;
			item.mana = 6;
			item.width = 50;
			item.height = 50;
			item.useTime = 27;
			item.useAnimation = 27;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 0;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 50, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("GildedProj1");
            item.shootSpeed = 20f;
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MarbleChunk", 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 0f, mod.ProjectileType("GildedProj2"), (int)(damage * 1), knockBack, player.whoAmI);
                return false;
            }
            else
            {

                int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = 40;
                item.useAnimation = 40;
                item.damage = 22;
                item.shootSpeed = 15;
                item.mana = 10;
                item.knockBack = 1;
                item.autoReuse = false;
            }
            else
            {
                item.useTime = 24;
                item.useAnimation = 24;
                item.shootSpeed = 6.2f;
                item.damage = 16;
				                item.mana = 6;
                item.knockBack = 1;
                item.autoReuse = true;
            }
            return base.CanUseItem(player);
        }
    }
}
