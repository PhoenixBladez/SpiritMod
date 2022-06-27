using SpiritMod.Projectiles.Thrown;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class Coconut : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hard Coconut");
			Tooltip.SetDefault("Does more damage if dropped from high up\n'You're not brave enough to try eating it'");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 9;
			Item.height = 15;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Ranged;
			Item.channel = true;
			Item.noMelee = true;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.shoot = ModContent.ProjectileType<CoconutP>();
			Item.useAnimation = 60;
			Item.useTime = 60;
			Item.shootSpeed = 4f;
			Item.damage = 21;
			Item.knockBack = 3.5f;
			Item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
			Item.crit = 8;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
	}
}
