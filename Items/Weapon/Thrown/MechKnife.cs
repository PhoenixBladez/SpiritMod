using SpiritMod.Projectiles.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class MechKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechanical Scrap");
			Tooltip.SetDefault("Hits enemies multiple times, burning them");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 19;
			item.height = 15;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.ranged = true;
			item.channel = true;
			item.noMelee = true;
			item.consumable = true;
			item.maxStack = 999;
			item.shoot = ModContent.ProjectileType<MechKnifeProj>();
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 12f;
			item.damage = 42;
			item.knockBack = 4.5f;
			item.value = Item.buyPrice(0, 0, 6, 0);
			item.rare = ItemRarityID.Pink;
			item.autoReuse = true;
			item.consumable = true;
		}
	}
}