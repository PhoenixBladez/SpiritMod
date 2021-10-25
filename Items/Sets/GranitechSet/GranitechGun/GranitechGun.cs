using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGun : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Granitech Blaster");

		public override void SetDefaults()
		{
			item.damage = 37;
			item.width = 28;
			item.height = 14;
			item.useTime = item.useAnimation = 9;
			item.knockBack = 0f;
			item.shootSpeed = 9;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.ranged = true;
			item.channel = true;
			item.value = Item.sellPrice(0, 1, 32, 0);
			item.rare = ItemRarityID.Orange;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.PurificationPowder;
			item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = ModContent.ProjectileType<GranitechGunProjectile>();

			Projectile.NewProjectileDirect(position, Vector2.Zero, type, 0, 0, player.whoAmI, player.altFunctionUse);
			return false;
		}
	}
}