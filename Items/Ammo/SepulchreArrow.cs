
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
	public class SepulchreArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Accursed Arrow");
			Tooltip.SetDefault("Pierces up to two enemies");
		}


		public override void SetDefaults()
		{
			item.width = 8;
			item.height = 16;
			item.value = 80;
			item.rare = ItemRarityID.Blue;
			item.value = Item.buyPrice(0, 0, 0, 30);
			item.maxStack = 999;

			item.damage = 9;
			item.knockBack = 2f;
			item.ammo = AmmoID.Arrow;

			item.ranged = true;
			item.consumable = true;

			item.shoot = ModContent.ProjectileType<AccursedArrow>();
			item.shootSpeed = 3.7f;

		}
	}
}