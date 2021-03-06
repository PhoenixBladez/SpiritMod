using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
	public class PlagueSkulls : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Skulls");
			Tooltip.SetDefault("The longer you use it, the faster it gets, but less dangerous each one becomes");
		}


		public override void SetDefaults()
		{
			item.reuseDelay = 60;
			item.width = 36;
			item.height = 36;
			item.value = Item.buyPrice(0, 2, 6, 0);
			item.rare = ItemRarityID.Yellow;
			item.crit += 6;
			item.damage = 72;
			item.mana = 1;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 60;
			item.useAnimation = 60;
			item.reuseDelay = 5;
			item.magic = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<PlagueSkullsHandle>();
			item.shootSpeed = 26f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Material.CursedFire>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}