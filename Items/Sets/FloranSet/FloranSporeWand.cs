using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.FloranSet
{
	public class FloranSporeWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Spore Wand");
			Tooltip.SetDefault("Shoots out a floating Floran Spore\nHit enemies are occasionally ensnared by vines and lose speed");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 46;
			item.value = Item.buyPrice(0, 0, 20, 0);
			item.rare = 1;
			item.damage = 15;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.useTime = 28;
			item.useAnimation = 28;
			item.mana = 6;
			item.knockBack = 3;
			item.UseSound = SoundID.Item20;
			item.magic = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<FloranSpore>();
			item.shootSpeed = 10f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<FloranBar>(), 14);
			modRecipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 5);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
	}
}
