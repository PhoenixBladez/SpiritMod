
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Fish
{
    public class HoneySalmon : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Honey-Glazed Salmon");
            Tooltip.SetDefault("Minor improvements to all stats\nBoosts life regeneration");
        }


        public override void SetDefaults() {
            item.width = item.height = 22;
            item.rare = 1;
            item.maxStack = 99;
            item.noUseGraphic = true;
            item.useStyle = 2;
            item.useTime = item.useAnimation = 30;

            item.buffType = BuffID.WellFed;
            item.buffTime = 36000;
            item.noMelee = true;
            item.consumable = true;
            item.UseSound = SoundID.Item2;
            item.autoReuse = false;

        }
        public override bool CanUseItem(Player player) {
            player.AddBuff(48, 1800);
            return true;
        }
        public override void AddRecipes() {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(null, "RawFish", 2);
            recipe1.AddIngredient(ItemID.BottledHoney, 1);
            recipe1.AddTile(TileID.CookingPots);
            recipe1.SetResult(this, 1);
            recipe1.AddRecipe();
        }
    }
}
