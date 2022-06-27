using SpiritMod.Projectiles.Yoyo;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Yoyo
{
	public class SweetThrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sweet Throw");
			Tooltip.SetDefault("Releases bees to chase down your foes");
		}


		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodYoyo);
			Item.damage = 25;
			Item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.knockBack = 2;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 25;
			Item.useTime = 27;
			Item.shoot = ModContent.ProjectileType<SweetThrowProjectile>();
		}
	}
}
