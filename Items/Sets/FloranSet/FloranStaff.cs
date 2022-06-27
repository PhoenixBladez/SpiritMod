using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.BriarDrops;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.FloranSet
{
	public class FloranStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Staff");
			Tooltip.SetDefault("Calls three guarding energies that surround the player before dissipating\nVines occasionally ensnare the foes, reducing their movement speed");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 50;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 17;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.mana = 12;
			Item.knockBack = 3;
			Item.DamageType = DamageClass.Magic;
			Item.UseSound = SoundID.Item20;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<FloranOrb>();
			Item.shootSpeed = 10f;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe();
			modRecipe.AddIngredient(ModContent.ItemType<FloranBar>(), 12);
			modRecipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 6);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			//Remove all previous Floran projectiles - creates "reset" behavior
			for (int i = 0; i < Main.projectile.Length; i++) {
				Projectile p = Main.projectile[i];
				if (p.active && p.type == Item.shoot && p.owner == player.whoAmI) {
					p.active = false;
				}
			}

			//get degrees from direction vector
			int dir = (int)(velocity.ToRotation() / (Math.PI / 180));
			int dir2 = dir + 120;
			int dir3 = dir - 120;

			//spawn the new projectiles
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Item.shoot, damage, knockback, player.whoAmI, 0, dir);
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Item.shoot, damage, knockback, player.whoAmI, 0, dir2);
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Item.shoot, damage, knockback, player.whoAmI, 0, dir3);
			return false;
		}
	}
}
