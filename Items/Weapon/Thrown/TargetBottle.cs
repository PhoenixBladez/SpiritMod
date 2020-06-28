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
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 9;
			item.height = 15;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.ranged = true;
			item.noMelee = true;
			item.consumable = true;
			item.maxStack = 999;
			item.shoot = ModContent.ProjectileType<Projectiles.Thrown.TargetBottle>();
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 10.5f;
			item.damage = 0;
			item.knockBack = 1.5f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 20);
			item.crit = 8;
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
		}
	}
}
