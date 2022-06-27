using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Clubs;

namespace SpiritMod.Items.Sets.ClubSubclass
{
    public class GoldClub : ModItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Golden Greathammer");

		public override void SetDefaults()
        {
            Item.channel = true;
            Item.damage = 20;
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
            Item.shoot = ModContent.ProjectileType<GoldClubProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}