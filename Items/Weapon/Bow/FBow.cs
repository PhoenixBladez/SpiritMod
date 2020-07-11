using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class FBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Bow");
			Tooltip.SetDefault("'Primitive, yet useful'");
		}



		public override void SetDefaults()
		{
			item.damage = 14; //This is the amount of damage the item does
			item.noMelee = true; //This makes sure the bow doesn't do melee damage
			item.ranged = true; //This causes your bow to do ranged damage
			item.width = 24; //Hitbox width
			item.height = 30; //Hitbox height
			item.useTime = 25; //How long it takes to use the weapon. If this is shorter than the useAnimation it will fire twice in one click.
			item.useAnimation = 27;  //The animations time length
			item.useStyle = ItemUseStyleID.HoldingOut; //The style in which the item gets used. 5 for bows.
			item.shoot = ProjectileID.Shuriken; //Makes the bow shoot arrows
			item.useAmmo = AmmoID.Arrow; //Makes the bow consume arrows
			item.knockBack = 1; //The amount of knockback the item has
			item.rare = 1; //The item's name color
			item.UseSound = SoundID.Item5; //Sound that gets played on use
			item.autoReuse = true; //if the Bow autoreuses or not
			item.shootSpeed = 10f; //The arrows speed when shot
			item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 4);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}