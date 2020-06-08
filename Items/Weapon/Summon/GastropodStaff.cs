using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
    public class GastropodStaff : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Gastropod Staff");
            Tooltip.SetDefault("Summons a Gastropod to shoot lasers at foes!");

        }


        public override void SetDefaults() {
            item.width = 42;
            item.height = 36;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 7;
            item.mana = 10;
            item.damage = 38;
            item.knockBack = 7;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 30;
            item.useAnimation = 30;
            item.summon = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<GastropodMinion>();
            item.buffType = ModContent.BuffType<GastropodMinionBuff>();
            item.buffTime = 3600;
            item.UseSound = SoundID.Item44;


        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override bool UseItem(Player player) {
            if(player.altFunctionUse == 2) {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            return player.altFunctionUse != 2;
            position = Main.MouseWorld;
            speedX = speedY = 0;
            return true;
        }
    }
}