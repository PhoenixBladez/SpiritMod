using SpiritMod.Projectiles.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.BossLoot.AvianDrops
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
			Item.width = 18;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 1;
			Item.damage = 22;
			Item.knockBack = 3;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 21;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.consumable = false;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Scapula>();
			Item.shootSpeed = 8;
			Item.UseSound = SoundID.Item1;
		}
	}
}