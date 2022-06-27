using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Arrow
{
	class MartianArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electrified Arrow");
			Tooltip.SetDefault("'Positively negative for enemies!'");
		}
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 30;
			Item.value = Item.buyPrice(0, 0, 3, 0);
			Item.rare = ItemRarityID.Cyan;

			Item.maxStack = 999;

			Item.damage = 16;
			Item.knockBack = 2f;
			Item.ammo = AmmoID.Arrow;

			Item.DamageType = DamageClass.Ranged;
			Item.consumable = true;

			Item.shoot = ModContent.ProjectileType<ElectricArrow>();
			Item.shootSpeed = 4f;
		}


	}
}
