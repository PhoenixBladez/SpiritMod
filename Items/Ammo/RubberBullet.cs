
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
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
			item.width = 8;
			item.height = 16;
			item.value = 80;
			item.rare = ItemRarityID.Blue;
			item.value = Item.buyPrice(0, 0, 0, 15);
			item.maxStack = 999;

			item.damage = 1;
			item.knockBack = 4.5f;
			item.ammo = AmmoID.Bullet;

			item.ranged = true;
			item.consumable = true;

			item.shoot = ModContent.ProjectileType<RubberBulletProj>();
			item.shootSpeed = 11f;

		}
	}
}