using SpiritMod.Projectiles.Yoyo;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class TheFireball : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fireball");
			Tooltip.SetDefault("Shoots out bouncing fireballs");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodYoyo);
			Item.damage = 21;
			Item.value = Terraria.Item.sellPrice(0, 0, 90, 0);
			Item.rare = ItemRarityID.Orange;
			Item.knockBack = 1;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 28;
			Item.useTime = 25;
			Item.shoot = ModContent.ProjectileType<FireballProj>();
		}
	}
}