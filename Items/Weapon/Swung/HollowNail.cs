using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Sword;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Swung
{
	public class HollowNail : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hollow Nail");
			Tooltip.SetDefault("Use it above enemies to bounce on them");
		}

		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 23;
			Item.useAnimation = 23;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.value = Item.sellPrice(0, 0, 0, 20);
			Item.shoot = ModContent.ProjectileType<NailProj>();
			Item.shootSpeed = 30f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			position += velocity;
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X / 30, velocity.Y / 30, type, damage, knockback, player.whoAmI, velocity.X, velocity.Y);

			if (Item.shoot == ModContent.ProjectileType<NailProj>())
				Item.shoot = ModContent.ProjectileType<NailProj2>();
			else
				Item.shoot = ModContent.ProjectileType<NailProj>();
			return false;
		}
	}
}