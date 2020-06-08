using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class Slugger : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("The Slugger");
            Tooltip.SetDefault("Right click to throw the hammer as a returning boomerang");
        }


        public override void SetDefaults() {
            item.damage = 33;
            item.melee = true;
            item.width = 36;
            item.height = 44;
            item.useTime = 47;
            item.useAnimation = 47;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 11;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.shoot = mod.ProjectileType("Slugger1");
            item.rare = 3;
            item.shootSpeed = 12f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override bool CanUseItem(Player player) {
            for(int i = 0; i < 1000; ++i) {
                if(Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) {
                    return false;
                }
            }
            if(player.altFunctionUse == 2) {
                item.noUseGraphic = true;
                item.shoot = mod.ProjectileType("Slugger1");
            } else {
                item.noUseGraphic = false;
                item.shoot = 0;
            }
            return base.CanUseItem(player);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if(Main.rand.Next(7) == 0) {
                int num780 = Dust.NewDust(new Vector2(player.itemLocation.X - 16f, player.itemLocation.Y - 14f * player.gravDir), 16, 16, 173, 0f, 0f, 100, default(Color), 1f);
                if(Main.rand.Next(3) != 0) {
                    Main.dust[num780].noGravity = true;
                }
                Dust obj23 = Main.dust[num780];
                obj23.velocity *= 0.3f;
                Dust expr_3672_cp_0 = Main.dust[num780];
                expr_3672_cp_0.velocity.Y = expr_3672_cp_0.velocity.Y - 1.5f;
                Main.dust[num780].position = player.RotatedRelativePoint(Main.dust[num780].position, true);
            }
        }
    }
}