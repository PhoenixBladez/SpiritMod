using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TalonHeaddress : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Apostle's Headdress");
            Tooltip.SetDefault("7% increased magic and ranged critical strike chance");
        }

        public override void SetDefaults() {
            item.width = 38;
            item.height = 26;
            item.value = 10000;
            item.rare = 3;
            item.defense = 3;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<TalonGarb>();
        }
        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Wind Spirits guide you, granting you double jumps\nMagic and ranged attacks ocassionally spawn feathers to attack foes.";
            player.doubleJumpCloud = true;
            player.GetSpiritPlayer().talonSet = true;
        }

        public override void UpdateEquip(Player player) {
            player.magicCrit += 7;
            player.rangedCrit += 7;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Talon>(), 10);
            recipe.AddIngredient(ModContent.ItemType<FossilFeather>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
