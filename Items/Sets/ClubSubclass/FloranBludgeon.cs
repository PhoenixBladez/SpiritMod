using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.ClubSubclass
{
    public class FloranBludgeon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Floran Bludgeon");
            Tooltip.SetDefault("Releases a shockwave along the ground");
        }

        public override void SetDefaults()
        {
            Item.channel = true;
            Item.damage = 19;
            Item.width = 58;
            Item.height = 58;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.crit = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 8;
			Item.useTurn = true;
			Item.value = Item.sellPrice(0, 0, 22, 0);
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.Clubs.FloranBludgeonProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
			var recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FloranSet.FloranBar>(), 15);
            recipe.AddIngredient(ModContent.ItemType<BriarDrops.EnchantedLeaf>(), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}