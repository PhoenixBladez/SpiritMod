using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
namespace SpiritMod.Items.Weapon.Magic
{
	public class FloranStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Staff");
			Tooltip.SetDefault("Calls three guarding energies that surround the player before dissipating\nVines occasionally ensnare the foes, reducing their movement speed");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 50;			
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = 2;
			item.damage = 17;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.useTime = 45;
			item.useAnimation = 45;
			item.mana = 12;
            item.knockBack = 3;
			item.magic = true;
            item.UseSound = SoundID.Item20;
            item.noMelee = true;
			item.shoot = mod.ProjectileType("FloranOrb");
			item.shootSpeed = 10f;
		}

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "FloranBar", 15);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//Remove all previous Floran projectiles - creates "reset" behavior
			for(int i = 0; i < Main.projectile.Length; i++) {
				Projectile p = Main.projectile[i];
				if(p.active && p.type == item.shoot && p.owner == player.whoAmI) {
					p.active = false;
				}
			}

			//get degrees from direction vector
			int dir = (int)(new Vector2(speedX, speedY).ToRotation() / (Math.PI / 180));
			int dir2 = dir + 120;
			int dir3 = dir - 120;

			//spawn the new projectiles
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, damage, knockBack, player.whoAmI, 0, dir);
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, damage, knockBack, player.whoAmI, 0, dir2);
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, damage, knockBack, player.whoAmI, 0, dir3);
			return false;
		}
    }
}
