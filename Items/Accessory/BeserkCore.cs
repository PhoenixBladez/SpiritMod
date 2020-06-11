using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class BeserkCore : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Core of the Berserker");
            Tooltip.SetDefault("Increases armor penetration by 6\n5% increased melee damage and critical strike chance\nHitting foes may cause them to release a cloud of gas\nMelee critical hits may cause foes to explode\n12% increased melee damage and speed when underground\n7% increased melee damage and speed when under half health\nReduces damage taken by 4%\nOccasionally nullifies hostile projectiles");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
        }


        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.defense = 2;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            if(player.ZoneRockLayerHeight) {
                player.meleeDamage += .12f;
                player.meleeSpeed += .12f;
            }
            player.GetSpiritPlayer().wheezeScale = true;
            player.meleeCrit += 5;
            player.armorPenetration += 6;
            player.GetSpiritPlayer().infernalFlame = true;
            player.meleeDamage += .05f;
            if(player.statLife <= player.statLifeMax2 / 2) {
                player.meleeDamage += 0.07f;
                player.meleeSpeed += 0.07f;
            }
            player.GetSpiritPlayer().atmos = true;
            player.endurance += 0.04f;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<WheezerScale>(), 1);
            recipe.AddIngredient(ModContent.ItemType<FieryTrident>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Atmos>(), 1);
            recipe.AddIngredient(ModContent.ItemType<FossilFlower>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SunShard>(), 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }

    }
}
