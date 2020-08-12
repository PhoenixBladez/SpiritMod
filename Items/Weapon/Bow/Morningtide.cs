using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class Morningtide : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morningtide");
			Tooltip.SetDefault("Converts wooden arrows into Dawnstrike Shafts");
		}



		public override void SetDefaults()
		{
			item.damage = 55;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 38;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 5;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.DD2_GhastlyGlaiveImpactGhost;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.autoReuse = true;
			item.shootSpeed = 16f;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<MorningtideProjectile>();
			}
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}