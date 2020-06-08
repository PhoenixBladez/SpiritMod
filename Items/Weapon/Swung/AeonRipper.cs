using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class AeonRipper : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Aeon Ripper");
            Tooltip.SetDefault("'Feel the wrath'");
        }


        public override void SetDefaults() {
            item.damage = 250;
            item.melee = true;
            item.width = 34;
            item.height = 22;
            item.useTime = 8;
            item.autoReuse = true;
            item.useAnimation = 8;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 40, 0, 0);
            item.rare = 12;
            item.UseSound = SoundID.Item1;
            item.crit = 33;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) {
            target.AddBuff(ModContent.BuffType<SoulFlare>(), 180);
        }
    }
}