using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.SpiritSet
{
	public class PhantomArc : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Arc");
			Tooltip.SetDefault("Summons an infinitely piercing laser of lost souls");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.useTurn = false;
			Item.value = Item.buyPrice(0, 6, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.damage = 34;
			Item.mana = 9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 10;
			Item.useAnimation = 7;
			//item.scale = 0.9f;
			Item.reuseDelay = 5;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true;
			Item.noMelee = true;
			//item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<PhantomArcHandle>();
			Item.shootSpeed = 26f;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 8);
			modRecipe.AddIngredient(ModContent.ItemType<SoulShred>(), 6);
			modRecipe.AddIngredient(ItemID.SpellTome, 1);
			modRecipe.AddTile(TileID.Bookcases);
			modRecipe.Register();
		}
	}
}
