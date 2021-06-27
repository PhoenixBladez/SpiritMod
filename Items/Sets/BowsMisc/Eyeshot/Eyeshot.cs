using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.Eyeshot
{
	public class Eyeshot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eyeshot");
			Tooltip.SetDefault("Wooden Arrows turn into Eyeballs!");
		}



		public override void SetDefaults()
		{
			item.damage = 14;
			item.noMelee = true;
			item.ranged = true;
			item.width = 50;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 4;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shootSpeed = 4.25f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<EyeArrow>();
			}
				Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			return false;

		}
	}
}
