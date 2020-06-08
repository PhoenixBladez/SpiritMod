using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
    public class BlueEyeStaff : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Staff of the Moongazer");
            Tooltip.SetDefault("Summons a Watchful Eye to shoot lasers at foes\nOnly one eye can exist at once");
        }


        public override void SetDefaults() {
            item.damage = 39;
            item.summon = true;
            item.mana = 9;
            item.width = 44;
            item.height = 48;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = 20000;
            item.rare = 5;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<EyeballBlue>();
            item.shootSpeed = 0f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            //remove any other owned SpiritBow projectiles, just like any other sentry minion
            for(int i = 0; i < Main.projectile.Length; i++) {
                Projectile p = Main.projectile[i];
                if(p.active && p.type == item.shoot && p.owner == player.whoAmI) {
                    p.active = false;
                }
            }
            //projectile spawns at mouse cursor
            Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = value18;
            return true;
        }
    }
}