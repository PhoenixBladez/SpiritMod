using SpiritMod.Projectiles.Yoyo;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Martian : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrestrial Ultimatum");
			Tooltip.SetDefault("Shoots electrospheres");
		}



		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodYoyo);
			Item.damage = 124;
			Item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.knockBack = 4;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 28;
			Item.useTime = 25;
			Item.shoot = ModContent.ProjectileType<MartianP>();
		}
	}
}