using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class PartyStarter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Partystarter");
			Tooltip.SetDefault("'Let's get this party started!'\nConverts regular bullets into VIP party bullets");
		}


		public override void SetDefaults()
		{
			item.damage = 70;
			item.ranged = true;
			item.width = 65;
			item.height = 32;
			item.useTime = 50;
			item.useAnimation = 50;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 12;
			item.useTurn = false;
			item.value = Terraria.Item.buyPrice(0, 19, 99, 0);
			item.rare = ItemRarityID.Pink;
			item.crit = 10;
			item.UseSound = SoundID.Item40;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<PartyStarterBullet>();
			item.shootSpeed = 17f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.Bullet) {
				type = ModContent.ProjectileType<PartyStarterBullet>();
			}
			return true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}