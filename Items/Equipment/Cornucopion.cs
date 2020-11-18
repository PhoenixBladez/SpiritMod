using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
    public class Cornucopion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cornucop-ion");
            Tooltip.SetDefault("Hold to charge up lightning that strikes nearby enemies\nCharging up for longer periods creates more strikes\nCharging up for too long electrifies the player\nCan only be used on the surface or higher\n'Shockingly effective'");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Equipment/Cornucopion_Glow");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.knockBack = 8;
            item.noMelee = true;
            item.useTurn = true;
            item.channel = true; //Channel so that you can held the weapon [Important]
            item.rare = ItemRarityID.Pink;
            item.width = 18;
            item.height = 18;
            item.useTime = 20;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MoonWizardHorn");
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.expert = true;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<CornucopionProj>();
            item.shootSpeed = 0f;
            item.noUseGraphic = false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .28f, .38f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Equipment/Cornucopion_Glow"),
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
        public override bool CanUseItem(Player player)
        {
            if (!player.ZoneOverworldHeight && !player.ZoneSkyHeight)
            {
                return false;
            }
            return true;
        }
    }
}
