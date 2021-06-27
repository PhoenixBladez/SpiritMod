using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class TrueDarkStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Horizon's Edge");
			Tooltip.SetDefault("Shoots out a splitting volley of homing pestilence and cursed fire.");
		}


		public override void SetDefaults()
		{
			item.damage = 62;
			item.magic = true;
			item.mana = 15;
			item.width = 66;
			item.height = 68;
			item.useTime = 34;
			item.useAnimation = 34;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item92;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Magic.CursedFire>();
			item.shootSpeed = 16f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddIngredient(ModContent.ItemType<NightStaff>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}