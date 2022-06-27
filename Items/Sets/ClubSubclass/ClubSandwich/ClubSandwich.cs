using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Projectiles.Clubs;

namespace SpiritMod.Items.Sets.ClubSubclass.ClubSandwich
{
    public class ClubSandwich : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Club Sandwich");
            Tooltip.SetDefault("Fully charged slams release sandwich bits\nCollect bits to boost stats");
        }

        public override void SetDefaults()
        {
            Item.channel = true;
            Item.damage = 19;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.crit = 4;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 12;
            Item.useTurn = true;
            Item.value = Item.sellPrice(0, 1, 42, 0);
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<ClubSandwichProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Baguette>(), 1);
            recipe.AddIngredient(ModContent.ItemType<CaesarSalad>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}