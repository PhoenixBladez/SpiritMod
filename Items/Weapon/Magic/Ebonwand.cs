using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class Ebonwand : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Wand of Hexing");
            Tooltip.SetDefault("Inflicts Cursed Inferno");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/Ebonwand_Glow");
        }
        public override bool CloneNewInstances => true;


        public override void SetDefaults() {
            item.damage = 7;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 26;
            item.height = 26;
            item.useTime = 34;
            item.mana = 4;
            item.useAnimation = 34;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 1f;
            item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shootSpeed = 6;
            item.shoot = ModContent.ProjectileType<EbonwandProj>();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 50f;
            if(Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
                position += muzzleOffset;
            }
            return true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/Ebonwand_Glow"),
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
    }
}
