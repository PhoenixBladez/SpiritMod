using SpiritMod.Projectiles.Yoyo;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Ancient : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient");
			Tooltip.SetDefault("Shoots a cluster of Ancient Ice");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodYoyo);
			Item.damage = 104;
			Item.value = Terraria.Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.knockBack = 3;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 28;
			Item.useTime = 25;
			Item.shoot = ModContent.ProjectileType<AncientP>();
		}
	}
}