using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Sets.ClubSubclass
{
    public class Blasphemer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Blasphemer");
            Tooltip.SetDefault("Charged strikes create a fiery geyser");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/ClubSubclass/Blasphemer_Glow");
        }

        public override void SetDefaults()
        {
            Item.channel = true;
            Item.damage = 35;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.crit = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 10;
            Item.useTurn = true;
            Item.value = Item.sellPrice(0, 0, 90, 0);
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.Clubs.BlasphemerProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Items.Sets.SlagSet.CarvedRock>(), 25);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(Item.position, 0.245f, .126f, .066f);
            Texture2D texture;
            texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw
            (
                Mod.Assets.Request<Texture2D>("Items/Sets/ClubSubclass/Blasphemer_Glow").Value,
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
    }
}