using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class ScarabSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Longhorn Blade");
			Tooltip.SetDefault("Kicks up waves of sand and dust");
		}


		public override void SetDefaults()
		{
			item.damage = 17;
			item.melee = true;
			item.width = 50;
			item.autoReuse = true;
			item.height = 50;
			item.useTime = 109;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<ScarabProjectile>();
			item.shootSpeed = 6; ;
		}
	}
}