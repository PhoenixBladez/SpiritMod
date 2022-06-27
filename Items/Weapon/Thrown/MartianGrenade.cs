using SpiritMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class MartianGrenade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electrosphere Grenade");
			Tooltip.SetDefault("'WARNING- HIGH VOLTAGE'");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Grenade);
			Item.shoot = ModContent.ProjectileType<ElectrosphereGrenade>();
			Item.useAnimation = 30;
			Item.rare = ItemRarityID.Yellow;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 34;
			Item.damage = 110;
			Item.value = 1900;
		}
	}
}