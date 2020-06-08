using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
    public class PinaColada : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Pina Colada");
            Tooltip.SetDefault("'Life is good'");
        }


        public override void SetDefaults() {
            item.width = 20;
            item.height = 30;
            item.rare = 2;
            item.maxStack = 30;

            item.useStyle = 2;
            item.useTime = item.useAnimation = 20;

            item.consumable = true;
            item.autoReuse = false;
            item.buffType = BuffID.WellFed;
            item.buffTime = 25000;

            item.UseSound = SoundID.Item3;
        }

        public override bool UseItem(Player player) {
            player.AddBuff(146, 25000); //happy
            player.AddBuff(BuffID.Calm, 25000);
            if(Main.hardMode) {
                player.AddBuff(BuffID.Tipsy, 25000);
            }
            return true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Coconut>(), 5);
            recipe.AddTile(94); //keg
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
