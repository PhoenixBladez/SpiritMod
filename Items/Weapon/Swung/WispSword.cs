using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class WispSword : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shadowisp Sword");
            Tooltip.SetDefault("Shoots out a spread of slowing shadow bolts");
        }


        int charger;
        public override void SetDefaults() {
            item.damage = 54;
            item.useTime = 26;
            item.useAnimation = 26;
            item.melee = true;
            item.width = 50;
            item.height = 50;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
            item.shootSpeed = 11;
            item.UseSound = SoundID.Item100;
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = ModContent.ProjectileType<ShadowWisp>();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox) {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 173);
                Main.dust[dust].noGravity = true;
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            {
                for(int I = 0; I < 2; I++) {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 80), speedY + ((float)Main.rand.Next(-230, 230) / 80), ModContent.ProjectileType<ShadowWisp>(), damage, knockBack, player.whoAmI, 0f, 0f);
                }
                charger = 0;
            }
            return false;
        }
    }
}