using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Held;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
	public class RunicDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Runic Drill");
		}


		public override void SetDefaults()
		{
			item.width = 54;
			item.height = 26;
			item.rare = ItemRarityID.Pink;
			item.value = 20000;

			item.pick = 180;

			item.damage = 23;
			item.knockBack = 0;

			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 7;
			item.useAnimation = 25;

			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.noUseGraphic = true;

			item.shoot = ModContent.ProjectileType<RunicDrillProjectile>();
			item.shootSpeed = 40f;

			item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Rune>(), 12);
			recipe.AddIngredient(ModContent.ItemType<SoulShred>(), 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
