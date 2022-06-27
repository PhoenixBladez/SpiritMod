using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritBiomeDrops
{
	public class StarCutter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Cutter");
		}


		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 26;
			Item.height = 26;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Ranged;
			Item.channel = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<StarCutterProj>();
			Item.useAnimation = 25;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.useTime = 25;
			Item.shootSpeed = 14f;
			Item.damage = 39;
			Item.knockBack = 3f;
			Item.value = Item.sellPrice(0, 0, 4, 50);
			Item.rare = ItemRarityID.Pink;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
	}
}
