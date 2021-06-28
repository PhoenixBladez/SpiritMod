using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.HeavenFleet
{
	public class HeavenFleet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heaven Fleet");
			Tooltip.SetDefault("Converts regular bullets into bouncing stars \nHold down for a bigger blast");

		}

		public override void SetDefaults()
		{
			item.channel = true;
			item.damage = 45;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 3;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 1, 42, 0);
			item.rare = ItemRarityID.Orange;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<HeavenFleetProj>();
			item.shootSpeed = 25f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<ConfluxPellet>();
			}
			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<HeavenFleetProj>(), damage, knockBack, player.whoAmI, type);
			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}