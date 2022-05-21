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
			item.shoot = ModContent.ProjectileType<Projectiles.Magic.SoullessSolution>() - ProjectileID.PureSpray;
			item.ammo = AmmoID.Solution;
			item.width = 10;
			item.height = 12;
			item.value = Item.buyPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.Orange;
			item.maxStack = 999;
			item.consumable = true;
		}
	}
}
