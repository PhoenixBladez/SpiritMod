using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.ClubSubclass
{
    public class WoodenClub : ModItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Wooden Club");

		public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 11;
            item.width = 60;
            item.height = 60;
            item.useTime = 320;
            item.useAnimation = 320;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 8;
            item.useTurn = true;
            item.value = Item.sellPrice(0, 0, 1, 0);
            item.rare = ItemRarityID.White;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<Projectiles.Clubs.WoodClubProj>();
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 25);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}