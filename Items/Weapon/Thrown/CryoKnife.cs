using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class CryoKnife : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Bomb");
			Tooltip.SetDefault("Occasionally inflicts 'Cryo Crush'\n'Cryo Crush' does more damage as enemy health wanes\nThis effect does not apply to bosses, and deals a flat amount of damage instead");
		}


		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Shuriken);
			Item.width = 30;
			Item.height = 30;
			Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.CryoKnife>();
			Item.useAnimation = 55;
			Item.useTime = 55;
			Item.shootSpeed = 10f;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 16;
			Item.autoReuse = true;
			Item.knockBack = 1f;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.LightRed;
		}
	}
}
