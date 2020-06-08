using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class CactusStaff : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cactus Staff");
            Tooltip.SetDefault("Shoots two cactus needles at foes\nThese pins stick to enemies and poison them");
        }


        public override void SetDefaults() {
            item.damage = 12;
            item.magic = true;
            item.mana = 7;
            item.width = 40;
            item.height = 40;
            item.useTime = 11;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 2f;
            item.value = 200;
            item.rare = 1;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 8);
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<CactusProj>();
            item.shootSpeed = 8f;
            item.autoReuse = false;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            for(int k = 0; k < 15; k++) {
                Vector2 offset = mouse - player.Center;
                offset.Normalize();
                if(speedX > 0) {
                    offset = offset.RotatedBy(-0.1f);
                } else {
                    offset = offset.RotatedBy(0.1f);
                }
                offset *= 58f;
                int dust = Dust.NewDust(player.Center + offset, 1, 1, 39);
                Main.dust[dust].noGravity = true;
                float dustSpeed = Main.rand.Next(9) / 5;
                switch(Main.rand.Next(2)) {
                    case 0:
                        Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed).RotatedBy(1.57f);
                        break;
                    case 1:
                        Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed).RotatedBy(-1.57f);
                        break;
                }
            }

            return true;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cactus, 22);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
