using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.ClubSubclass
{
    public class PlatinumClub : ModItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Platinum Scepter");

		public override void SetDefaults()
        {
            Item.channel = true;
            Item.damage = 19;
            Item.width = 58;
            Item.height = 58;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.crit = 6;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 9;
            Item.useTurn = true;
            Item.value = Item.sellPrice(0, 0, 22, 0);
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.Clubs.PlatinumClubProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}