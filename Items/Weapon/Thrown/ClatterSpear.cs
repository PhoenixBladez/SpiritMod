using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Thrown.Charge;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
	public class ClatterSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatter Javelin");
			Tooltip.SetDefault("Hold and release to throw\nHold it longer for more velocity and damage \nAttacks occasionally lowering enemy defense");
		}


		public override void SetDefaults()
		{
			item.damage = 14;
			item.noMelee = true;
			item.channel = true; //Channel so that you can held the weapon [Important]
			item.rare = 3;
			item.width = 18;
			item.height = 18;
			item.useTime = 15;
			item.useAnimation = 45;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 24;
			item.knockBack = 8;
			item.melee = true;
			item.noMelee = true;
			//   item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<ClatterJavelinProj>();
			item.shootSpeed = 0f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Carapace>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}