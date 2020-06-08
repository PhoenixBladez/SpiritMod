using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
    public class SnakeStaff : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Lihzahrd Wand");
            Tooltip.SetDefault("Summons a friendly Flying Snake to shoot venom at foes");

        }


        public override void SetDefaults() {
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.mana = 13;
            item.damage = 44;
            item.knockBack = 1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 30;
            item.useAnimation = 30;
            item.summon = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<SnakeMinion>();
            item.buffType = ModContent.BuffType<SnakeMinionBuff>();
            item.buffTime = 3600;
            item.UseSound = SoundID.Item44;
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            return player.altFunctionUse != 2;
        }

        public override bool UseItem(Player player) {
            if(player.altFunctionUse == 2) {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }
}