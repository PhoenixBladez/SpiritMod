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
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.BriarSolution>() - ProjectileID.PureSpray;
			Item.ammo = AmmoID.Solution;
			Item.width = 10;
			Item.height = 12;
            Item.value = Item.buyPrice(0, 0, 7, 0);
            Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
			Item.consumable = true;
		}
	}
}
