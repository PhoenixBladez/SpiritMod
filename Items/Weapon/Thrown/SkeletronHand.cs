using SpiritMod.Projectiles.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class SkeletronHand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Cutter");
			Tooltip.SetDefault("Dashes through the air in rapid bursts");
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
			Item.shoot = ModContent.ProjectileType<SkeletronHandProj>();
			Item.useAnimation = 27;
			Item.useTime = 27;
			Item.shootSpeed = 12f;
			Item.damage = 19;
			Item.knockBack = 3.5f;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.crit = 4;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = false;
			Item.consumable = true;
		}
	}
}