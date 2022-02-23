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
            item.channel = true;
            item.damage = 19;
            item.width = 58;
            item.height = 58;
            item.useTime = 320;
            item.useAnimation = 320;
            item.crit = 6;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 9;
            item.useTurn = true;
            item.value = Item.sellPrice(0, 0, 22, 0);
            item.rare = ItemRarityID.Blue;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<Projectiles.Clubs.PlatinumClubProj>();
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}