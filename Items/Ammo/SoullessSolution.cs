using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
	class SoullessSolution : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grey Solution");
			Tooltip.SetDefault("Used by the Clentaminator\nPurifies The Spirit and The Briar");
		}

		public override void SetDefaults()
		{
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.SoullessSolution>() - ProjectileID.PureSpray;
			Item.ammo = AmmoID.Solution;
			Item.width = 10;
			Item.height = 12;
			Item.value = Item.buyPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
			Item.consumable = true;
		}
	}
}
