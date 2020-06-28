using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
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
			item.width = 24;
			item.height = 28;
			item.useTurn = false;
			item.value = Item.buyPrice(0, 6, 0, 0);
			item.rare = 5;
			item.damage = 34;
			item.mana = 9;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 10;
			item.useAnimation = 7;
			//item.scale = 0.9f;
			item.reuseDelay = 5;
			item.magic = true;
			item.channel = true;
			item.noMelee = true;
			//item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<PhantomArcHandle>();
			item.shootSpeed = 26f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 8);
			modRecipe.AddIngredient(ModContent.ItemType<SoulShred>(), 6);
			modRecipe.AddIngredient(ItemID.SpellTome, 1);
			modRecipe.AddTile(TileID.Bookcases);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
