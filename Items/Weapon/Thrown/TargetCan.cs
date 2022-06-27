using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class TargetCan : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Target Can");
			Tooltip.SetDefault("Hit it with a bullet in the air to do extremely high damage \n'Let's see what kind of a shot ya are, pilgrim!'");
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
			Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.TargetCan>();
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 8.5f;
			Item.damage = 0;
			Item.knockBack = 1.5f;
			Item.value = Terraria.Item.sellPrice(0, 0, 0, 20);
			Item.crit = 8;
			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(5);
			recipe.AddIngredient(ItemID.TinCan, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
