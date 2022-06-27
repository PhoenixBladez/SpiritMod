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
            Item.channel = true;
            Item.damage = 11;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.crit = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 8;
            Item.useTurn = true;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.White;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.Clubs.WoodClubProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 25);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}