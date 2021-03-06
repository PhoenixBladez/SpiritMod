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
			item.CloneDefaults(ItemID.WoodYoyo);
			item.damage = 21;
			item.value = Terraria.Item.sellPrice(0, 0, 90, 0);
			item.rare = ItemRarityID.Orange;
			item.knockBack = 1;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 28;
			item.useTime = 25;
			item.shoot = ModContent.ProjectileType<FireballProj>();
		}
	}
}