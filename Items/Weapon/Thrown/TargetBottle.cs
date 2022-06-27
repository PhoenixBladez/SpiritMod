using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class TargetBottle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Target Bottle");
			Tooltip.SetDefault("Hit it with a bullet in the air to do high bullet damage for a short time after\n'Take a crack at this bottle!'");
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 9;
			Item.height = 15;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.TargetBottle>();
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 10.5f;
			Item.damage = 0;
			Item.knockBack = 1.5f;
			Item.value = Terraria.Item.sellPrice(0, 0, 0, 20);
			Item.crit = 8;
			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
	}
}
