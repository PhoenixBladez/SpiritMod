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
            item.channel = true;
            item.damage = 16;
            item.width = 58;
            item.height = 58;
            item.useTime = 320;
            item.useAnimation = 320;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 8;
            item.useTurn = false;
            item.value = Item.sellPrice(0, 0, 22, 0);
            item.rare = ItemRarityID.Blue;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("FloranBludgeonProj");
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
			var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Sets.FloranSet.FloranBar>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Sets.BriarDrops.EnchantedLeaf>(), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}