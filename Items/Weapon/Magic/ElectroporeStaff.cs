using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
	public class ElectroporeStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electropore Conduit");
			Tooltip.SetDefault("Shoots out a burst of celestial electricity that leaves behind lingering stars");
		}

		public override void SetDefaults()
		{
			item.width = 54;
			item.height = 50;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.damage = 47;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.useTime = 22;
			item.useAnimation = 22;
			item.mana = 9;
			item.knockBack = 2;
			item.magic = true;
			item.UseSound = SoundID.Item9;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<StarOrb>();
			item.shootSpeed = 15f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EelRod>(), 1);
			recipe.AddIngredient(ModContent.ItemType<AstralLens>(), 1);
			recipe.AddIngredient(ModContent.ItemType<StellarBar>(), 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}
