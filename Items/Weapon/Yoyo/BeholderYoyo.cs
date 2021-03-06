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
			item.CloneDefaults(ItemID.WoodYoyo);
			item.damage = 19;
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.knockBack = 3.5f;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 25;
			item.mana = 5;
			item.useTime = 25;
			item.shoot = ModContent.ProjectileType<BeholderYoyoProj>();
			item.magic = true;
			item.channel = true;
			item.melee = false;
		}
	}
}