using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Sword;

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
			item.damage = 15;
			item.melee = true;
			item.noMelee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 23;
			item.useAnimation = 23;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 5;
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.value = Item.sellPrice(0, 0, 0, 20);
			item.shoot = ModContent.ProjectileType<NailProj>();
			item.shootSpeed = 30f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += new Vector2(speedX, speedY);
			Projectile.NewProjectile(position.X, position.Y, speedX / 30, speedY / 30, type, damage, knockBack, player.whoAmI, speedX, speedY);

			if (item.shoot == ModContent.ProjectileType<NailProj>())
				item.shoot = ModContent.ProjectileType<NailProj2>();
			else
				item.shoot = ModContent.ProjectileType<NailProj>();
			return false;
		}
	}
}