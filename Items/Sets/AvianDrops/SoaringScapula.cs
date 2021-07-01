using SpiritMod.Projectiles.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.AvianDrops
{
	public class SoaringScapula : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soaring Scapula");
			Tooltip.SetDefault("Pulls enemies towards the ground");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 20;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.Green;
			item.maxStack = 1;
			item.damage = 22;
			item.knockBack = 3;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 21;
			item.ranged = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.consumable = false;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<Scapula>();
			item.shootSpeed = 8;
			item.UseSound = SoundID.Item1;
		}
	}
}