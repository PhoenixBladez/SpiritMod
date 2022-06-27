using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GladeWraithDrops
{
	public class OakHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oak Heart");
			Tooltip.SetDefault("Hitting foes may cause poisonous spores to rain down\nPoisons hit foes");
		}


		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 9;
			Item.height = 15;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.maxStack = 1;
			Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.OakHeart>();
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 5f;
			Item.damage = 12;
			Item.knockBack = 1.5f; ;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			Item.consumable = false;
		}
	}
}