
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Bullet
{
	public class RubberBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rubber Bullet");
			Tooltip.SetDefault("Knocks enemies back farther");
		}


		public override void SetDefaults()
		{
			Item.width = 8;
			Item.height = 16;
			Item.value = 80;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 0, 15);
			Item.maxStack = 999;

			Item.damage = 1;
			Item.knockBack = 4.5f;
			Item.ammo = AmmoID.Bullet;

			Item.DamageType = DamageClass.Ranged;
			Item.consumable = true;

			Item.shoot = ModContent.ProjectileType<RubberBulletProj>();
			Item.shootSpeed = 11f;

		}
	}
}