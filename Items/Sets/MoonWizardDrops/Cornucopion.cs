using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MoonWizardDrops
{
    public class Cornucopion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cornucop-ion");
            Tooltip.SetDefault("Hold to charge up lightning that strikes nearby enemies\nCharging up for longer periods creates more strikes\nCharging up for too long electrifies the player\nCan only be used on the surface or higher\n'Shockingly effective'");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/MoonWizardDrops/Cornucopion_Glow");
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.knockBack = 8;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.channel = true; //Channel so that you can held the weapon [Important]
            Item.rare = ItemRarityID.Pink;
            Item.width = 18;
            Item.height = 18;
            Item.useTime = 20;
            Item.UseSound = Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MoonWizardHorn");
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.expert = true;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<CornucopionProj>();
            Item.shootSpeed = 0f;
            Item.value = 10000;
            Item.noUseGraphic = false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(Item.position, 0.08f, .28f, .38f);
            Texture2D texture;
            texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw
            (
                ModContent.Request<Texture2D>("SpiritMod/Items/Sets/MoonWizardDrops/Cornucopion_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
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
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -1);
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
