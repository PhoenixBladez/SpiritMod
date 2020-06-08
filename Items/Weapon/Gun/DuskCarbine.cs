using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Gun
{
    public class DuskCarbine : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dusk Carbine");
            Tooltip.SetDefault("Converts regular bullets into Shadowflame bullets");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Gun/DuskCarbine_Glow");
        }


        public override void SetDefaults() {
            item.width = 54;
            item.height = 28;
            item.rare = 5;
            item.UseSound = SoundID.Item11;
            item.crit = 4;
            item.damage = 37;
            item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);

            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useTime = 9;
            item.useAnimation = 9;
            item.reuseDelay = 16;

            item.ranged = true;
            item.autoReuse = true;
            item.useAmmo = AmmoID.Bullet;

            item.shoot = 10;
            item.shootSpeed = 8;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Weapon/Gun/DuskCarbine_Glow"),
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
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(type == ProjectileID.Bullet)
                type = ModContent.ProjectileType<ShadowflameBullet>();

            return true;
        }

        public override bool ConsumeAmmo(Player player) {
            return player.itemAnimation >= item.useAnimation - 2;
        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-10, 0);
        }
    }
}
