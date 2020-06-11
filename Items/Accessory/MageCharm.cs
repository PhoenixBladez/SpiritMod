
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class MageCharm : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Flame of the Magus");
            Tooltip.SetDefault("7% increased magic damage\n10% increased magic critical strike chance\n6% reduced mana usage\nIncreases maximum mana by 50\nIncreases mana regeneration\nMagic critical hits may deal extra damage\nMagic attacks may spawn sparks that deal more damage the less mana you have\nMagic attacks may slow down enemies");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 5));
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }


        public override void SetDefaults() {
            item.width = 22;
            item.height = 54;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 8;
            item.defense = 1;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().eyezorEye = true;
            player.GetSpiritPlayer().manaWings = true;
            player.GetSpiritPlayer().winterbornCharmMage = true;
            player.statManaMax2 += 50;
            player.magicDamage += 0.07f;
            player.magicCrit += 10;
            player.manaRegenBonus += 2;
            if(!hideVisual) Lighting.AddLight(player.position, 0.0f, .75f, 1.25f);
            player.manaCost -= 0.06f;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<ManaFlame>(), 1);
            recipe.AddIngredient(ModContent.ItemType<WintryCharmMage>(), 1);
            recipe.AddIngredient(ItemID.ManaRegenerationBand, 1);
            recipe.AddIngredient(ModContent.ItemType<FallenAngel>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Eyezor>(), 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
    }
}
