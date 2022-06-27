using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Projectiles.Summon.Zones;
using SpiritMod.Buffs.Zones;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.ArcaneZoneSubclass
{
	public class LowGravCodex : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Codex: Low Gravity Zone");
			Tooltip.SetDefault("Summons a low gravity zone at the cursor position\nLow gravity zones decrease player gravity and increase aerial acceleration\nZones count as sentries");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/ArcaneZoneSubclass/LowGravCodex_Glow");
        }

        public override void SetDefaults()
		{
			Item.damage = 0;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 54;
			Item.height = 50;
			Item.useTime = 31;
			Item.useAnimation = 31;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.DD2_EtherianPortalSpawnEnemy;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<LowGravZone>();
			Item.shootSpeed = 0f;
			Item.buffType = ModContent.BuffType<LowGravZoneTimer>();
			Item.buffTime = Projectile.SentryLifeTime;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			position = Main.MouseWorld;
            Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            player.UpdateMaxTurrets();
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.1f, .1f, .1f);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.GetTexture("Items/Sets/ArcaneZoneSubclass/LowGravCodex_Glow"),
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

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EmptyCodex>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SpaceJunkItem>(), 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}