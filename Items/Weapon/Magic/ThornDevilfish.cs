
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class ThornDevilfish : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Thorn Devilfish");
            Tooltip.SetDefault("Shoots out poisonous bubbles");
        }



        public override void SetDefaults() {
            item.damage = 11;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 22;
            item.mana = 6;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
            item.rare = 2;
            item.autoReuse = false;
            item.shootSpeed = 7;
            item.UseSound = SoundID.Item111;
            item.shoot = 523;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 523, damage, knockBack, player.whoAmI);
            Main.projectile[p].timeLeft = 60;
            Main.projectile[p].scale *= .6f;
            Main.projectile[p].magic = true;
            Main.projectile[p].ranged = false;
            return false;
        }
    }
}
