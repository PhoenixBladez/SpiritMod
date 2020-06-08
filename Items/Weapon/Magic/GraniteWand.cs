using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class GraniteWand : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Unstable Conduit");
            Tooltip.SetDefault("Creates a fountain of energy at the ground near the Cursor Position\nKilling enemies with this weapon causes them to explode into damaging energy wisps");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/GraniteWand_Glow");
        }


        public override void SetDefaults() {
            item.damage = 17;
            item.magic = true;
            item.mana = 15;
            item.width = 44;
            item.height = 44;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 4;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.crit = 10;
            item.UseSound = SoundID.Item109;
            item.shoot = ModContent.ProjectileType<GraniteWandProj>();
            item.shootSpeed = 8f;
            item.autoReuse = false;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            for(int i = 0; i < Main.projectile.Length; i++) {
                Projectile p = Main.projectile[i];
                if(p.active && p.type == item.shoot && p.owner == player.whoAmI) {
                    p.active = false;
                }
            }
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 100f, type, damage, knockBack, player.whoAmI);
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Lighting.AddLight(item.position, 0.08f, .12f, .52f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Magic/GraniteWand_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GraniteChunk", 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
