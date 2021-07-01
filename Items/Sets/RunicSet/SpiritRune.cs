using SpiritMod.Items.Sets.SpiritSet;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.RunicSet
{
	public class SpiritRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Rune");
			Tooltip.SetDefault("'Contains ancient energy' \n Shoots out an ancient book filled with dangerous runes");
		}


		public override void SetDefaults()
		{
			item.damage = 43;
			item.magic = true;
			item.mana = 20;
			item.width = 28;
			item.height = 32;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<RuneBook>();
			item.shootSpeed = 2f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Rune>(), 8);
			recipe.AddIngredient(ModContent.ItemType<SoulShred>(), 4);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
