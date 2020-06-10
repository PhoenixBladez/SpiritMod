using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FrigidArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class FrigidHelm : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Frigid Faceplate");

        }


        int timer = 0;
        public override void SetDefaults() {
            item.width = 28;
            item.height = 24;
            item.value = 1100;
            item.rare = 1;
            item.defense = 3;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<FrigidChestplate>() && legs.type == ModContent.ItemType<FrigidLegs>();
        }
        public override void UpdateArmorSet(Player player) {
            Player closest = Main.player[(int)Player.FindClosest(player.position, player.width, player.height)];
            player.setBonus = "Double tap down to create an icy wall at the cursor position\n8 second cooldown";
            player.GetSpiritPlayer().frigidSet = true;

            if(Main.rand.Next(6) == 0) {
                int dust = Dust.NewDust(player.position, player.width, player.height, 187);
                Main.dust[dust].noGravity = true;
            }
        }


        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
