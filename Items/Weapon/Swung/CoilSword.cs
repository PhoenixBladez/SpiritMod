using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class CoilSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Blade");
			Tooltip.SetDefault("Every four successful hits on an enemy releases an electrical explosion");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/CoilSword_Glow");
        }


        public override void SetDefaults()
        {
            item.damage = 23;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .12f, .52f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Swung/CoilSword_Glow"),
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
        int charger;
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            charger++;
            if (charger >= 4)
            {
                Main.PlaySound(2, (int)target.position.X, (int)target.position.Y, 14);
                Terraria.Projectile.NewProjectile(target.Center.X, target.Center.Y, 0f, 0f, mod.ProjectileType("CoiledExplosion"), damage, knockBack, player.whoAmI);
                charger = 0;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "TechDrive", 4);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
