using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Typhoon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Typhoon");
			Tooltip.SetDefault("Shoots sharks at nearby enemies");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 26;
			Item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Purple;
			Item.crit += 4;
			Item.damage = 115;
			Item.knockBack = 4f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Yoyo.Typhoon>();
			Item.UseSound = SoundID.Item1;
		}
	}
}
