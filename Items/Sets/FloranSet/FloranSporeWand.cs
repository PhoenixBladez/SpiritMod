using SpiritMod.Items.Sets.BriarDrops;
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
			Item.width = 30;
			Item.height = 46;
			Item.value = Item.buyPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.mana = 6;
			Item.knockBack = 3;
			Item.UseSound = SoundID.Item20;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<FloranSpore>();
			Item.shootSpeed = 10f;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe();
			modRecipe.AddIngredient(ModContent.ItemType<FloranBar>(), 14);
			modRecipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 5);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}
