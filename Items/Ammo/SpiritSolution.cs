
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritSolnAmmo = SpiritMod.Projectiles.Magic.SpiritSolution;
namespace SpiritMod.Items.Ammo
{
	class SpiritSolution : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cyan Solution");
			Tooltip.SetDefault("Used by the Clentaminator\nSpreads the Spirit");
		}

		public override void SetDefaults()
		{
			Item.shoot = ModContent.ProjectileType<SpiritSolnAmmo>() - ProjectileID.PureSpray;
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
