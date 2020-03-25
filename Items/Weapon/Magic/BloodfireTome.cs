using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class BloodfireTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of Blood Sacrifice");
			Tooltip.SetDefault("Shoots sticking possessed daggers at enemies");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/BloodfireTome_Glow");
        }


		public override void SetDefaults()
		{
			item.damage = 24;
			item.magic = true;
			item.mana = 9;
			item.useTime = 26;
			item.useAnimation = 28;
            item.width = item.height = 26;
            item.useStyle = 5;

			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("PossessedDagger");
			item.shootSpeed = 14f;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/BloodfireTome_Glow"),
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
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 12);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}