using SpiritMod.Projectiles.Yoyo;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class BeholderYoyo : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye of the Beholder");
			Tooltip.SetDefault("Consumes 15 mana per second\nInflicts magic damage");
		}



		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodYoyo);
			Item.damage = 19;
			Item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.knockBack = 3.5f;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 25;
			Item.mana = 5;
			Item.useTime = 25;
			Item.shoot = ModContent.ProjectileType<BeholderYoyoProj>();
			Item.DamageType = DamageClass.Magic;
			Item.channel = true;
		}
	}
}