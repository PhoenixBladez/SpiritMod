using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CoiledMask : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Autonaut's Headgear");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/CoiledMask_Glow");
        }

        public override void SetDefaults() {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
            item.vanity = true;
            item.rare = 2;
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) {
            glowMaskColor = Color.White;
        }
    }
}
