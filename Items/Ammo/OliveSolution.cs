using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Items.Ammo
{
	class OliveSolution : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Olive Solution");
			Tooltip.SetDefault("Used by the Clentaminator\nSpreads the Briar");
		}

		public override void SetDefaults()
		{
			item.shoot = ModContent.ProjectileType<Projectiles.Magic.BriarSolution>() - ProjectileID.PureSpray;
			item.ammo = AmmoID.Solution;
			item.width = 10;
			item.height = 12;
            item.value = Item.buyPrice(0, 0, 7, 0);
            item.rare = ItemRarityID.Orange;
			item.maxStack = 999;
			item.consumable = true;
		}
	}
}
