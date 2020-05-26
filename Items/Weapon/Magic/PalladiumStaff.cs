using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class PalladiumStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Staff");
			Tooltip.SetDefault("Summons a runic pillar at the cursor position\nIf below 1/3 HP, step inside the pillar to rapidly regenerate health");
		}


		public override void SetDefaults()
		{
			item.damage = 40;
			item.magic = true;
			item.mana = 15;
			item.width = 40;
			item.height = 40;
			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 5;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("PalladiumStaffProj");
			item.shootSpeed = 8f;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == item.shoot && p.owner == player.whoAmI)
                {
                    p.active = false;
                }
            }
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 100f, type, damage, knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(1184, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
