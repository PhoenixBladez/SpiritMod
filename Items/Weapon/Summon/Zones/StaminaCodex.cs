using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Armor;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Summon.Zones;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.Zones
{
	public class StaminaCodex : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Codex: Stamina Zone");
			Tooltip.SetDefault("Summons a stamina zone at the cursor position\nStamina zones greatly increase movement speed and agility\nZones count as sentries");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Summon/Zones/StaminaCodex_Glow");
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.058f, .105f, .066f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Summon/Zones/StaminaCodex_Glow"),
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

        public override void SetDefaults()
		{
			item.damage = 0;
			item.summon = true;
			item.mana = 24;
			item.width = 54;
			item.height = 50;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 0;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.DD2_EtherianPortalSpawnEnemy;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<StaminaZone>();
			item.shootSpeed = 0f;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool AltFunctionUse(Player player)
		{
			return true;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 value18 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			position = value18;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            player.UpdateMaxTurrets();
			return false;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<EmptyCodex>(), 1);
            recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 5);
            recipe.AddIngredient(ItemID.JungleSpores, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}