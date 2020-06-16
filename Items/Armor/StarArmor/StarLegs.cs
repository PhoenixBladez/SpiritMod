using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class StarLegs : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Astralite Leggings");
            Tooltip.SetDefault("Increases ranged damage by 5%\nLeave a trail of stars where you walk");
        }
        int timer = 0;
        public override void SetDefaults() {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 35, 0);
            item.rare = 3;
            item.defense = 7;
        }
        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += .05f;
            if (player.velocity.X != 0f)
            {
                int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, 226);
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].scale *= .4f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void ArmorSetShadows(Player player) {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<SteamParts>(), 8);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
