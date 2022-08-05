using SpiritMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritBiomeDrops
{
	public class GhostJellyBomb : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Ghost Jelly Bomb");

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Shuriken);
			Item.width = 37;
			Item.height = 26;
			Item.shoot = ModContent.ProjectileType<GhostJellyBombProj>();
			Item.useAnimation = 27;
			Item.useTime = 27;
			Item.shootSpeed = 11f;
			Item.damage = 35;
			Item.knockBack = 1.0f;
			Item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
			Item.crit = 6;
			Item.rare = ItemRarityID.Pink;
			Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = false;
		}
	}
}
